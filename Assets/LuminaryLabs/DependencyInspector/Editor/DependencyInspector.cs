//-----------------------------------------------------------------------
// Dependency Inspector Tool (Extended for Nested Classes)
// Description: 
//   Scans the current scene for missing object references (including in 
//   serializable sub-classes) and UnityEvent fields on components (only 
//   those visible in the Inspector) and displays them. Foldout states 
//   are preserved across refreshes using an in-memory dictionary.
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LuminaryLabs.Tools
{
    /// <summary>
    /// The main editor window for scanning and displaying missing dependencies and UnityEvents.
    /// </summary>
    public class DependencyInspector : EditorWindow
    {
        #region Fields

        // all need namespace add here
        private static readonly string[] AllowedNamespaces = { "_Project" };

        private Vector2 _scrollPos;
        private List<DependencyEntry> _dependencyEntries = new List<DependencyEntry>();

        // Tracks the user’s pending assignment of objects to missing fields:
        // Key = (component, propertyPath), Value = new reference
        private readonly Dictionary<(Component comp, string propertyPath), Object> _pendingAssignments = new();

        // Dictionary to preserve foldout states (keyed by GameObject EntityId)
        private static Dictionary<EntityId, bool> _foldoutStates = new Dictionary<EntityId, bool>();

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Opens the Dependency Inspector window.
        /// </summary>
        [MenuItem("Tools/Luminary Labs/Dependency Inspector")]
        public static void ShowWindow()
        {
            var window = GetWindow<DependencyInspector>("Dependency Inspector");
            window.Show();
        }

        private void OnEnable()
        {
            // Perform initial scan of the scene.
            _dependencyEntries = ScanScene();

            // Subscribe to hierarchy changes to refresh the dependency list when scene changes occur.
            EditorApplication.hierarchyChanged += RefreshDependencies;
        }

        private void OnDisable()
        {
            EditorApplication.hierarchyChanged -= RefreshDependencies;
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawButtons();
            DrawDependencyList();
        }

        #endregion

        #region GUI Methods

        /// <summary>
        /// Draws the header for the inspector window.
        /// </summary>
        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Dependency Inspector", EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Refresh", GUILayout.Width(100)))
            {
                RefreshDependencies();
            }

            if (GUILayout.Button("Collapse All", GUILayout.Width(100)))
            {
                foreach (var entry in _dependencyEntries)
                {
                    EntityId id = entry.GameObject.GetEntityId();
                    _foldoutStates[id] = false;
                }
            }

            if (GUILayout.Button("Expand All", GUILayout.Width(100)))
            {
                foreach (var entry in _dependencyEntries)
                {
                    EntityId id = entry.GameObject.GetEntityId();
                    _foldoutStates[id] = true;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        
        private void DrawButtons()
        {
            if (GUILayout.Button("Scan Scene"))
            {
                _dependencyEntries = ScanScene();
                _pendingAssignments.Clear();
            }
            /*if (GUILayout.Button("Fill Dependencies"))
            {
                ApplyPendingAssignments();
            }*/
        }
        

        /// <summary>
        /// Draws the list of dependency entries in a scrollable view.
        /// </summary>
        private void DrawDependencyList()
        {
            // Clean up any invalid entries.
            for (int i = _dependencyEntries.Count - 1; i >= 0; i--)
            {
                if (_dependencyEntries[i] == null ||
                    _dependencyEntries[i].GameObject == null ||
                    (_dependencyEntries[i].ComponentMissingItems.Count == 0 && _dependencyEntries[i].UnityEventItems.Count == 0))
                {
                    EntityId id = _dependencyEntries[i].GameObject ? _dependencyEntries[i].GameObject.GetEntityId() : EntityId.None;
                    if (_foldoutStates.ContainsKey(id))
                        _foldoutStates.Remove(id);
                    _dependencyEntries.RemoveAt(i);
                }
            }

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            foreach (var entry in _dependencyEntries)
            {
                DrawDependencyEntry(entry);
            }
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Draws an individual dependency entry.
        /// </summary>
        /// <param name="entry">The dependency entry to draw.</param>
        private void DrawDependencyEntry(DependencyEntry entry)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            DrawEntryHeader(entry);
            EditorGUILayout.EndHorizontal();

            if (entry.isToggleOpen)
            {
                EditorGUILayout.Space(3);
                // Get a union of all components that have missing references or UnityEvent fields.
                HashSet<Component> allComponents = new HashSet<Component>();
                foreach (var comp in entry.ComponentMissingItems.Keys)
                    allComponents.Add(comp);
                foreach (var comp in entry.UnityEventItems.Keys)
                    allComponents.Add(comp);

                // Draw each component with its missing items and/or UnityEvent fields.
                foreach (Component comp in allComponents)
                {
                    List<MissingReferenceItem> missingItems = entry.ComponentMissingItems.ContainsKey(comp)
                        ? entry.ComponentMissingItems[comp]
                        : new List<MissingReferenceItem>();

                    List<MissingReferenceItem> unityEventItems = entry.UnityEventItems.ContainsKey(comp)
                        ? entry.UnityEventItems[comp]
                        : new List<MissingReferenceItem>();

                    DrawComponent(comp, missingItems, unityEventItems);
                }
            }
            else
            {
                EditorGUILayout.Space(2);
            }

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the header for an entry (foldout and Ping button), preserving foldout state.
        /// </summary>
        /// <param name="entry">The dependency entry whose header to draw.</param>
        private void DrawEntryHeader(DependencyEntry entry)
        {
            EntityId id = entry.GameObject.GetEntityId();
            bool isOpen = true;
            if (!_foldoutStates.TryGetValue(id, out isOpen))
            {
                isOpen = true; // Default to open if not stored.
            }

            isOpen = EditorGUILayout.Foldout(isOpen, entry.GameObject.name, true);
            _foldoutStates[id] = isOpen;
            entry.isToggleOpen = isOpen;

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Ping", GUILayout.Width(50)))
            {
                EditorGUIUtility.PingObject(entry.GameObject);
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a component’s missing object references and its UnityEvent fields.
        /// </summary>
        /// <param name="comp">The component being inspected.</param>
        /// <param name="missingItems">List of missing reference items.</param>
        /// <param name="unityEventItems">List of UnityEvent items.</param>
        private void DrawComponent(Component comp, List<MissingReferenceItem> missingItems, List<MissingReferenceItem> unityEventItems)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            if (comp == null)
            {
                EditorGUILayout.LabelField("Missing Script");
                EditorGUILayout.EndVertical();
                return;
            }

            GUIContent componentContent = EditorGUIUtility.ObjectContent(comp, comp.GetType());
            string componentName = ObjectNames.NicifyVariableName(comp.GetType().Name);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Label(componentContent.image, GUILayout.Width(15), GUILayout.Height(15));
            EditorGUILayout.LabelField(componentName, EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            SerializedObject so = new SerializedObject(comp);

            // Separate non-list items from those that share a bracket path
            Dictionary<string, List<MissingReferenceItem>> listGroups = new Dictionary<string, List<MissingReferenceItem>>();
            List<MissingReferenceItem> standaloneItems = new List<MissingReferenceItem>();

            foreach (var item in missingItems)
            {
                int bracketIndex = item.PropertyPath.IndexOf('[');
                if (bracketIndex > 0)
                {
                    // Group anything with bracket notation (e.g., "options[0].image")
                    string root = item.PropertyPath.Substring(0, bracketIndex);
                    if (!listGroups.ContainsKey(root))
                        listGroups[root] = new List<MissingReferenceItem>();
                    listGroups[root].Add(item);
                }
                else
                {
                    // Everything else is a standalone field
                    standaloneItems.Add(item);
                }
            }

            // Draw all standalone fields
            foreach (var item in standaloneItems)
            {
                DrawMissingReferenceItem(comp, item);
            }

            // Draw grouped items for lists
            foreach (var group in listGroups)
            {
                DrawMissingReferenceListGroup(comp, group.Key, group.Value);
            }

            // Finally, draw UnityEvent fields (if any)
            if (unityEventItems.Count > 0)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Unity Events", EditorStyles.boldLabel);
                foreach (var item in unityEventItems)
                {
                    DrawUnityEvent(comp, so, item);
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }


        private void DrawMissingReferenceListGroup(Component comp, string groupName, List<MissingReferenceItem> items)
        {
            EditorGUILayout.BeginVertical();

            // A header label for the group
            //EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(groupName) + " (Collection)", EditorStyles.boldLabel);

            // Create a temporary ReorderableList (non-reorderable)
            var rList = new UnityEditorInternal.ReorderableList(items, typeof(MissingReferenceItem), false, true, true, true);

            // Set up how the header is drawn
            rList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, NicifyPropertyPath(groupName));
            };

            // Set up how each element is drawn
            rList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                // Safely fetch the sub-item
                if (index < 0 || index >= items.Count) return;
                MissingReferenceItem subItem = items[index];

                // Resolve the field type and current value
                System.Type fieldType = GetFieldType(comp, subItem.PropertyPath) ?? typeof(Object);
                (Component, string) key = (comp, subItem.PropertyPath);
                Object currentPendingValue = _pendingAssignments.ContainsKey(key)
                    ? _pendingAssignments[key]
                    : GetFieldValue(comp, subItem.PropertyPath) as Object;

                // Draw a label and an object field side by side
                float labelWidth = 150f;
                Rect labelRect = new Rect(rect.x, rect.y + 1, labelWidth, EditorGUIUtility.singleLineHeight);
                Rect fieldRect = new Rect(rect.x + labelWidth + 5, rect.y + 1, rect.width - labelWidth - 5, EditorGUIUtility.singleLineHeight);

                // A simple label for the element (e.g., "Element 0 - Image")
                string elementLabel = NicifyListElementLabel(groupName, subItem.PropertyPath);
                EditorGUI.LabelField(labelRect, elementLabel);

                EditorGUI.BeginChangeCheck();
                Object newValue = EditorGUI.ObjectField(fieldRect, currentPendingValue, fieldType, allowSceneObjects: true);
                if (EditorGUI.EndChangeCheck())
                {
                    _pendingAssignments[key] = newValue;
                    SetFieldValue(comp, subItem.PropertyPath, newValue);
                    EditorUtility.SetDirty(comp);
                }
            };

            // Adjust the element height slightly for spacing
            rList.elementHeight = EditorGUIUtility.singleLineHeight + 4;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20); // Adds 20 pixels of left space
            rList.DoLayoutList();
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.EndVertical();
        }

        private string NicifyListElementLabel(string groupName, string propertyPath)
        {
            // Example: "options[2].icon" => "Element 2 - Icon"
            // You can customize this logic further if needed
            int startBracket = propertyPath.IndexOf('[');
            int endBracket = propertyPath.IndexOf(']');
            if (startBracket >= 0 && endBracket > startBracket)
            {
                string indexStr = propertyPath.Substring(startBracket + 1, endBracket - startBracket - 1);
                string remainder = propertyPath.Substring(endBracket + 1).TrimStart('.');
                string nicifiedRemainder = string.IsNullOrEmpty(remainder)
                    ? ""
                    : " - " + ObjectNames.NicifyVariableName(remainder);
                return $"Element {indexStr}{nicifiedRemainder}";
            }
            return ObjectNames.NicifyVariableName(propertyPath);
        }

        private void DrawMissingReferenceItem(Component comp, MissingReferenceItem item)
        {
            // Use the stored type if available; otherwise fall back to reflection
            System.Type fieldType = item.FieldType
                ?? GetFieldType(comp, item.PropertyPath)
                ?? typeof(Object);

            (Component, string) key = (comp, item.PropertyPath);
            Object currentPendingValue = _pendingAssignments.ContainsKey(key)
                ? _pendingAssignments[key]
                : GetFieldValue(comp, item.PropertyPath) as Object;

            // Build a more readable display name
            string displayName = NicifyPropertyPath(item.PropertyPath);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(displayName, GUILayout.Width(200));

            EditorGUI.BeginChangeCheck();
            Object newValue = EditorGUILayout.ObjectField(
                currentPendingValue,
                fieldType,
                allowSceneObjects: true,
                GUILayout.MinWidth(100)
            );
            if (EditorGUI.EndChangeCheck())
            {
                _pendingAssignments[key] = newValue;
                SetFieldValue(comp, item.PropertyPath, newValue);
                EditorUtility.SetDirty(comp);
            }

            EditorGUILayout.EndHorizontal();
        }




        private string NicifyPropertyPath(string path)
        {
            // Example: "options[2].icon" => "Options[2] > Icon"
            string[] parts = path.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                int bracketPos = parts[i].IndexOf('[');
                if (bracketPos >= 0)
                {
                    // Split out the bracketed portion
                    string baseName = parts[i].Substring(0, bracketPos);
                    string bracketPart = parts[i].Substring(bracketPos);
                    parts[i] = ObjectNames.NicifyVariableName(baseName) + bracketPart;
                }
                else
                {
                    parts[i] = ObjectNames.NicifyVariableName(parts[i]);
                }
            }
            return string.Join(" > ", parts);
        }


        /// <summary>
        /// Draws a UnityEvent field using Unity’s default property field.
        /// This should include functionality for adding and removing event listeners.
        /// </summary>
        /// <param name="comp">The component containing the UnityEvent.</param>
        /// <param name="so">The SerializedObject for the component.</param>
        /// <param name="item">The UnityEvent item.</param>
        private void DrawUnityEvent(Component comp, SerializedObject so, MissingReferenceItem item)
        {
            so.Update();

            SerializedProperty eventProp = so.FindProperty(item.PropertyPath);
            if (eventProp == null)
            {
                EditorGUILayout.LabelField($"{ObjectNames.NicifyVariableName(item.PropertyPath)} (not found)");
                return;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(
                eventProp,
                new GUIContent(ObjectNames.NicifyVariableName(item.PropertyPath)),
                true);

            if (EditorGUI.EndChangeCheck())
            {
                so.ApplyModifiedProperties();
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Returns the System.Type of the field with the given property path on the component.
        /// This handles nested fields (e.g., "myField.mySubField[3].anotherField").
        /// </summary>
        private System.Type GetFieldType(Component comp, string propertyPath)
        {
            // We resolve the final field/element in the chain
            // and return its type.
            (object parentObject, FieldInfo fieldInfo, int? index) = ResolvePath(comp, propertyPath);
            if (fieldInfo == null)
                return null;

            // If this field is an array or list, and we have an index,
            // then the type is the element type. Otherwise, it's the field type.
            if (index.HasValue)
            {
                if (fieldInfo.FieldType.IsArray)
                {
                    return fieldInfo.FieldType.GetElementType();
                }
                else if (fieldInfo.FieldType.IsGenericType)
                {
                    return fieldInfo.FieldType.GetGenericArguments()[0];
                }
            }
            return fieldInfo.FieldType;
        }

        /// <summary>
        /// Gets the value of a field (including nested fields) or a list element on a component.
        /// </summary>
        private static object GetFieldValue(Component comp, string propertyPath)
        {
            (object parentObject, FieldInfo fieldInfo, int? index) = ResolvePath(comp, propertyPath);
            if (fieldInfo == null || parentObject == null) return null;

            object fieldValue = fieldInfo.GetValue(parentObject);
            if (index.HasValue && fieldValue is IList list && index.Value < list.Count)
            {
                return list[index.Value];
            }
            return fieldValue;
        }

        /// <summary>
        /// Sets the value of a field (including nested fields) or a list element on a component.
        /// </summary>
        private static void SetFieldValue(Component comp, string propertyPath, object value)
        {
            (object parentObject, FieldInfo fieldInfo, int? index) = ResolvePath(comp, propertyPath);
            if (fieldInfo == null || parentObject == null) return;

            if (index.HasValue)
            {
                // It's an array or list element.
                object fieldValue = fieldInfo.GetValue(parentObject);
                if (fieldValue is IList list && index.Value < list.Count)
                {
                    list[index.Value] = value;
                }
            }
            else
            {
                // It's a direct field.
                fieldInfo.SetValue(parentObject, value);
            }
        }

        /// <summary>
        /// Resolves a property path (like "myField.mySubField[2].icon") into
        /// the parent object, the final FieldInfo, and an optional list index.
        /// </summary>
        /// <param name="rootComponent">The MonoBehaviour or Component at the root.</param>
        /// <param name="path">The property path string.</param>
        /// <returns>(parentObject, fieldInfo, index)</returns>
        private static (object, FieldInfo, int?) ResolvePath(Component rootComponent, string path)
        {
            // We'll split on '.' or on bracketed indices.
            // Example path: "options[2].icon"
            //  => tokens: ["options[2]", "icon"]
            // For each token, we might have a bracket (list access).

            string[] tokens = path.Split('.');
            object currentObject = rootComponent; // Start with the actual component as the "parent"
            FieldInfo currentField = null;
            int? listIndex = null;

            Type currentType = rootComponent.GetType();

            for (int i = 0; i < tokens.Length; i++)
            {
                // Check if token has bracket
                string token = tokens[i];
                listIndex = null; // Reset each iteration

                int bracketPos = token.IndexOf('[');
                string fieldName = (bracketPos >= 0) ? token.Substring(0, bracketPos) : token;

                // Get the FieldInfo from currentType
                currentField = currentType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (currentField == null) return (null, null, null);

                // Move "currentObject" to that field's value
                object fieldValue = currentField.GetValue(currentObject);

                // If there's a bracket, parse the index
                if (bracketPos >= 0)
                {
                    int closePos = token.IndexOf(']', bracketPos);
                    if (closePos > bracketPos)
                    {
                        string indexStr = token.Substring(bracketPos + 1, closePos - bracketPos - 1);
                        if (int.TryParse(indexStr, out int idx))
                        {
                            listIndex = idx;
                        }
                    }
                }

                // If this is the last token, we stop. We'll let the caller
                // do the final get/set with the field info + index.
                if (i == tokens.Length - 1)
                    return (currentObject, currentField, listIndex);

                // Otherwise, we step deeper:
                // If it's a list/array and we have a bracket, move into that element
                if (listIndex.HasValue && fieldValue is IList list && listIndex.Value < list.Count)
                {
                    // currentObject now becomes that element
                    fieldValue = list[listIndex.Value];
                }

                if (fieldValue == null) return (null, null, null);

                // Move currentObject forward and update currentType
                currentObject = fieldValue;
                currentType = currentObject.GetType();
            }

            return (currentObject, currentField, listIndex);
        }




        /// <summary>
        /// Scans the current scene for GameObjects with missing object references and UnityEvent fields.
        /// It now also checks:
        ///  - arrays/lists for null elements,
        ///  - serializable sub-classes (nested objects) for null references.
        /// </summary>
        private static List<DependencyEntry> ScanScene()
        {
            var report = new List<DependencyEntry>();

            // Find all active GameObjects in the scene
            foreach (var go in FindObjectsByType<GameObject>())
            {
                // If you only want to scan scene objects (not prefabs in the project),
                // you might check "go.scene.IsValid()" etc.
                go.scene.IsValid();

                var entry = new DependencyEntry { GameObject = go };

                foreach (var comp in go.GetComponents<Component>())
                {
                    if (comp == null)
                    {
                        // Для отсутствующего скрипта используем "заглушку", 
                        // так как Dictionary не принимает null
                        Transform missingScriptKey = entry.GameObject.transform; // Используем Transform как ключ-заглушку
                        if (!entry.ComponentMissingItems.ContainsKey(missingScriptKey))
                            entry.ComponentMissingItems[missingScriptKey] = new List<MissingReferenceItem>();

                        entry.ComponentMissingItems[missingScriptKey].Add(new MissingReferenceItem { PropertyPath = "Missing Script" });
                        continue;
                    }

                    var missingItems = new List<MissingReferenceItem>();
                    var unityEventItems = new List<MissingReferenceItem>();

                    // Only consider MonoBehaviours for reflection-based scanning.
                    if (comp is MonoBehaviour)
                    {
                        // Recursively check the fields in this component

                        ScanFields(comp, comp, comp.GetType(), "", missingItems, unityEventItems);
                        
                    }

                    // Check native (non-MonoBehaviour) components explicitly
                    /*
                    var nativeMissingItems = new List<MissingReferenceItem>();
                    CheckNativeComponentDependencies(comp, nativeMissingItems);
                    missingItems.AddRange(nativeMissingItems);
                    */
                    if (missingItems.Count > 0)
                    {
                        entry.ComponentMissingItems[comp] = missingItems;
                    }
                    if (unityEventItems.Count > 0)
                    {
                        entry.UnityEventItems[comp] = unityEventItems;
                    }
                }

                if (entry.ComponentMissingItems.Count > 0 || entry.UnityEventItems.Count > 0)
                {
                    report.Add(entry);
                }
            }

            return report;
        }

        // Explicit check for native Unity components (e.g. joints) that don't expose fields via reflection.
        /// <summary>
        /// Explicit check for native Unity components (e.g. joints) that don't expose fields via reflection.
        /// </summary>
        private static void CheckNativeComponentDependencies(Component comp, List<MissingReferenceItem> missingItems)
        {
            // 3D Joints
            if (comp is FixedJoint fixedJoint)
            {
                if (fixedJoint.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody)
                    });
                }
            }
            else if (comp is HingeJoint hingeJoint)
            {
                if (hingeJoint.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody)
                    });
                }
            }
            else if (comp is SpringJoint springJoint)
            {
                if (springJoint.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody)
                    });
                }
            }
            else if (comp is CharacterJoint characterJoint)
            {
                if (characterJoint.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody)
                    });
                }
            }
            else if (comp is ConfigurableJoint configurableJoint)
            {
                if (configurableJoint.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody)
                    });
                }
            }
            // 2D Joints
            else if (comp is FixedJoint2D fixedJoint2D)
            {
                if (fixedJoint2D.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody2D)
                    });
                }
            }
            else if (comp is HingeJoint2D hingeJoint2D)
            {
                if (hingeJoint2D.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody2D)
                    });
                }
            }
            else if (comp is SpringJoint2D springJoint2D)
            {
                if (springJoint2D.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody2D)
                    });
                }
            }
            else if (comp is DistanceJoint2D distanceJoint2D)
            {
                if (distanceJoint2D.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody2D)
                    });
                }
            }
            else if (comp is SliderJoint2D sliderJoint2D)
            {
                if (sliderJoint2D.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody2D)
                    });
                }
            }
            else if (comp is RelativeJoint2D relativeJoint2D)
            {
                if (relativeJoint2D.connectedBody == null)
                {
                    missingItems.Add(new MissingReferenceItem
                    {
                        PropertyPath = "connectedBody",
                        FieldType = typeof(Rigidbody2D)
                    });
                }
            }
        }

        /// <summary>
        /// Recursively scans a given object’s fields to find:
        ///  - null references to UnityEngine.Object
        ///  - UnityEvent fields
        ///  - nested serializable objects that might have more references
        /// </summary>
        /// 

        private static bool IsAllowedType(Type t)
        {
            var ns = t.Namespace;
            if (string.IsNullOrEmpty(ns)) return false;
            foreach (var allowed in AllowedNamespaces)
                if (ns == allowed || ns.StartsWith(allowed + ".")) return true;
            return false;
        }
        private static void ScanFields(Component rootComp,
                               object currentObject,
                               Type currentType,
                               string pathPrefix,
                               List<MissingReferenceItem> missingItems,
                               List<MissingReferenceItem> unityEventItems)
        {
            if (currentObject == null || currentType == null)
                return;

            // Если это UnityEngine.Object, не углубляемся (кроме MonoBehaviour)
            /*
            if (typeof(UnityEngine.Object).IsAssignableFrom(currentType) && !(currentObject is MonoBehaviour))
                return;
            */
            // Корневой/рекурсивный MonoBehaviour — проверяем namespace
            if (currentObject is MonoBehaviour mb)
            {
                if (!IsAllowedType(mb.GetType()))
                    return;
            }
            else
            {
                // Вложенный объект (сериализуемый класс или UnityEngine.Object)
                if (typeof(UnityEngine.Object).IsAssignableFrom(currentType))
                    return;   // Transform, Rigidbody и т.п. — не сканируем

                if (!IsAllowedType(currentType))
                    return;   // Вложенный [Serializable] класс не из наших namespace — не сканируем
            }

            // Пропускаем примитивы и строки
            if (currentType.IsPrimitive || currentType == typeof(string))
                return;

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

            // Идем по цепочке наследования вверх, пока не дойдем до MonoBehaviour или System.Object
            Type typeToScan = currentType;
            while (typeToScan != null && typeToScan != typeof(MonoBehaviour) && typeToScan != typeof(object))
            {
                // Пропускаем типы из UnityEngine / UnityEditor
                var ns = typeToScan.Namespace;
                if (!string.IsNullOrEmpty(ns) && (ns.StartsWith("UnityEngine") || ns.StartsWith("UnityEditor")))
                    break;

                // Берем поля только объявленные непосредственно в текущем классе (DeclaredOnly)
                foreach (var field in typeToScan.GetFields(flags))
                {
                    bool isPublic = field.IsPublic;
                    bool isSerialized = field.GetCustomAttribute<SerializeField>() != null;
                    bool isHidden = field.GetCustomAttribute<HideInInspector>() != null;
                    if (!(isPublic || isSerialized) || isHidden)
                        continue;

                    Type fieldType = field.FieldType;
                    object fieldValue = field.GetValue(currentObject);

                    string thisPath = string.IsNullOrEmpty(pathPrefix)
                        ? field.Name
                        : (pathPrefix + "." + field.Name);

                    // 1) Прямая ссылка на UnityEngine.Object
                    if (typeof(UnityEngine.Object).IsAssignableFrom(fieldType))
                    {
                        if (fieldValue == null || fieldValue.Equals(null))
                        {
                            missingItems.Add(new MissingReferenceItem
                            {
                                PropertyPath = thisPath,
                                FieldType = fieldType
                            });
                        }
                    }
                    // 2) UnityEvent
                    else if (typeof(UnityEventBase).IsAssignableFrom(fieldType))
                    {
                        unityEventItems.Add(new MissingReferenceItem
                        {
                            PropertyPath = thisPath
                        });
                    }
                    // 3) Коллекции (массивы и списки)
                    else if (typeof(IEnumerable).IsAssignableFrom(fieldType) && fieldType != typeof(string))
                    {
                        Type elementType = null;
                        if (fieldType.IsArray)
                        {
                            elementType = fieldType.GetElementType();
                        }
                        else if (fieldType.IsGenericType)
                        {
                            elementType = fieldType.GetGenericArguments()[0];
                        }

                        if (elementType != null && fieldValue != null)
                        {
                            IList list = fieldValue as IList;
                            if (list != null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    object element = list[i];
                                    string elementPath = $"{thisPath}[{i}]";

                                    if (typeof(UnityEngine.Object).IsAssignableFrom(elementType))
                                    {
                                        if (element == null || element.Equals(null))
                                        {
                                            missingItems.Add(new MissingReferenceItem
                                            {
                                                PropertyPath = elementPath,
                                                FieldType = elementType
                                            });
                                        }
                                    }
                                    else
                                    {
                                        ScanFields(rootComp, element, element?.GetType(), elementPath, missingItems, unityEventItems);
                                    }
                                }
                            }
                        }
                    }
                    // 4) Вложенные сериализуемые классы/структуры
                    else if (fieldType.IsClass || fieldType.IsValueType)
                    {
                        if (fieldType.IsSerializable && fieldValue != null)
                        {
                            ScanFields(rootComp, fieldValue, fieldValue.GetType(), thisPath, missingItems, unityEventItems);
                        }
                    }
                }

                // Переходим к базовому классу
                typeToScan = typeToScan.BaseType;
            }
        }
        /// <summary>
        /// Refreshes dependency entries by rescanning the scene.
        /// </summary>
        private void RefreshDependencies()
        {
            _dependencyEntries = ScanScene();
            Repaint();
        }

        #endregion

        #region Data Classes

        /// <summary>
        /// Represents a dependency entry for a GameObject.
        /// </summary>
        public class DependencyEntry
        {
            public GameObject GameObject;
            public Dictionary<Component, List<MissingReferenceItem>> ComponentMissingItems { get; } = new();
            public Dictionary<Component, List<MissingReferenceItem>> UnityEventItems { get; } = new();
            public bool isToggleOpen = true;
        }

        /// <summary>
        /// Represents a missing reference or UnityEvent field.
        /// </summary>
        public class MissingReferenceItem
        {
            public string PropertyPath;
            public System.Type FieldType; // <--- NEW
        }


        #endregion
    }
}
