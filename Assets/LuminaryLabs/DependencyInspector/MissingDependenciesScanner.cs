using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LuminaryLabs.Tools
{
    public static class MissingDependenciesScanner
    {
        /// <summary>
        /// Scans all GameObjects in the active scene for missing serialized references.
        /// For public or [SerializeField] fields (as represented in the SerializedProperty iterator),
        /// any field of type UnityEngine.Object that is null is flagged.
        /// </summary>
        public static List<MissingDependencyEntry> ScanScene()
        {
            List<MissingDependencyEntry> report = new List<MissingDependencyEntry>();

            GameObject[] allGameObjects = Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allGameObjects)
            {
                MissingDependencyEntry entry = new MissingDependencyEntry();
                entry.GameObject = go;

                // Scan all MonoBehaviour components on this GameObject.
                Component[] comps = go.GetComponents<MonoBehaviour>();
                foreach (Component comp in comps)
                {
                    // If the component itself is missing (i.e. a missing script), record that.
                    if (comp == null)
                    {
                        if (!entry.ComponentMissingItems.ContainsKey(null))
                            entry.ComponentMissingItems[null] = new List<MissingReferenceItem>();
                        entry.ComponentMissingItems[null].Add(new MissingReferenceItem { PropertyPath = "Missing Script" });
                        continue;
                    }

                    SerializedObject so = new SerializedObject(comp);
                    SerializedProperty prop = so.GetIterator();
                    List<MissingReferenceItem> missingItems = new List<MissingReferenceItem>();

                    // Iterate through all visible serialized properties.
                    bool enterChildren = true;
                    while (prop.NextVisible(enterChildren))
                    {
                        enterChildren = false;
                        if (prop.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            // NEW: Flag this property as missing if its objectReferenceValue is null.
                            if (prop.objectReferenceValue == null)
                            {
                                missingItems.Add(new MissingReferenceItem { PropertyPath = prop.propertyPath });
                            }
                        }
                    }

                    if (missingItems.Count > 0)
                    {
                        entry.ComponentMissingItems.Add(comp, missingItems);
                    }
                }
                if (entry.ComponentMissingItems.Count > 0)
                {
                    report.Add(entry);
                }
            }
            return report;
        }
    }
}
