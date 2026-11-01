using System.Collections.Generic;
using UnityEngine;

namespace LuminaryLabs.Tools
{
    public class MissingDependencyEntry
    {
        // The GameObject where missing references were found.
        public GameObject GameObject;
        // For each component on the GameObject that has missing references,
        // we store a list of MissingReferenceItem which contains the property path.
        public Dictionary<Component, List<MissingReferenceItem>> ComponentMissingItems = new Dictionary<Component, List<MissingReferenceItem>>();
    }

    /// <summary>
    /// Represents a missing reference on a serialized field.
    /// We store the property path so we can later get and update the field via SerializedObject.
    /// </summary>
    public class MissingReferenceItem
    {
        public string PropertyPath;  // e.g., "dependency"
    }
}