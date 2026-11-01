Dependency Inspector & Missing Dependencies Scanner User Guide
Overview

The Dependency Inspector and Missing Dependencies Scanner tools are designed to help you quickly locate and resolve missing serialized references and UnityEvent fields within your Unity scene. The system automatically detects missing dependencies by scanning all GameObjects and their MonoBehaviour components. When a missing dependency is assigned a new value through the Inspector interface, the tool immediately updates the underlying serialized property without requiring any additional user action. This user guide explains how the system works, describes each script’s role, and outlines how to use the tool effectively.

Script Descriptions

MissingDependencyEntry & MissingReferenceItem
      * MissingDependencyEntry:
            This class represents the scan report for a single GameObject. Each entry holds a reference to the GameObject and includes a dictionary mapping each component (or a null key for missing scripts) to a list of missing references.
      * MissingReferenceItem:
            This class represents a missing reference on a serialized field. It stores the property path (e.g., "dependency") used to later access and update the field via a SerializedObject. The property path is key to automatically applying changes once the dependency is fixed.

MissingDependenciesScanner
   * ScanScene():
      This static method scans every GameObject in the active scene for missing serialized references. It iterates through all MonoBehaviour components attached to each GameObject using a SerializedProperty iterator.
            * If a component is missing (null), it flags the GameObject with a “Missing Script” entry.
            * For each visible serialized property of type UnityEngine.Object (excluding the built-in script reference "m_Script"), it checks whether the property’s value is null. If so, it creates a new MissingReferenceItem containing the property path.
      The method returns a list of MissingDependencyEntry objects, each corresponding to a GameObject with one or more missing dependencies.

Dependency Inspector Tool
         * User Interface:

      This EditorWindow (located in the package’s Editor folder) displays the results of the scene scan. The window organizes the information in a hierarchical view:
            * The header displays the title “Dependency Inspector” along with controls to refresh the scan and to collapse or expand all entries.
            * For each GameObject with missing dependencies, the tool displays its name (with a “Ping” button to highlight it in the Scene view) and foldable lists of components that have missing references or UnityEvent fields.
            * Automatic Resolution:
                  The current tool automatically applies any changes made by the user. When you assign a new reference to a missing dependency using the Inspector’s object field, the tool immediately updates the underlying SerializedProperty.
                              * This is achieved by storing each missing reference’s component reference and property path during scanning (see the additional fields in MissingReferenceItem).
                              * Once the user assigns a value, the system creates a SerializedObject for the owning component, locates the property using the stored path, and sets its objectReferenceValue to the new value. The changes are applied instantly, and the system immediately refreshes the dependency graph.

Usage Instructions
      1. Launching the Tool:
            * From the Unity menu, navigate to Tools → Missing Dependencies Helper. The Dependency Inspector window will open.
            * The header displays the title, along with a Refresh button and options to collapse or expand all entries.
      2. Scanning the Scene:
            * Click “Scan Scene” to begin scanning all GameObjects in the active scene for missing references.
            * The scanner inspects each MonoBehaviour’s serialized fields and flags any object reference that is null.
            * The report is displayed in a scrollable list. Each entry lists the GameObject’s name along with components that have missing dependencies.
            * Missing scripts are automatically indicated by the label “Missing Script.”
      3. Interacting with the Report:
            * Each GameObject entry includes a “Ping” button that highlights the object in the Scene view for easy location.
            * Expand or collapse entries using the provided foldout controls. The tool preserves these states across refreshes using an in-memory dictionary keyed by GameObject instance IDs.
      4. Automatically Resolving Dependencies:
            * When a dependency field is missing, you can assign a value through the provided ObjectField. As soon as a new reference is set, the tool automatically updates the corresponding SerializedProperty using the stored component reference and property path.
            * There is no separate “Fill Dependencies” button—the system immediately applies changes and refreshes the dependency graph to reflect the update.
      5. Refreshing the Scan:
            * After making changes to the scene (for example, assigning new references), you can click “Refresh” to re-scan and update the dependency report. The tool listens for hierarchy changes to help keep the report current.

Troubleshooting & Additional Notes

      * Field Serialization:
            Only fields that are public or marked with [SerializeField] (and not hidden with [HideInInspector]) are scanned. Make sure your dependency fields are declared appropriately.
      * Property Path Accuracy:
            The property path stored in MissingReferenceItem is critical for automatically applying changes. If fields are renamed or if complex data structures are used, the path may not match the current state. In such cases, manually re-scanning the scene may be required.
      * Performance Considerations:
            In very large scenes, scanning every GameObject and component can be time-consuming. Consider using the Refresh button selectively if performance becomes an issue.
      * UnityEvent Handling:
            The tool also scans for UnityEvent fields. These are drawn using Unity’s default property fields, which allow you to add or remove event listeners. Only visible UnityEvent fields are included.
      * Automatic Updates:
            With changes applied immediately upon assignment, the Dependency Inspector continuously reflects the current state of your scene’s dependencies. If a dependency is resolved, its corresponding entry will be updated (or removed, if no missing references remain) upon the next scan.