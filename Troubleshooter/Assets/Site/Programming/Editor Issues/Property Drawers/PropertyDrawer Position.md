## PropertyDrawer position

### Description
PropertyDrawers don't support IMGUI layout functionality. You must not use methods that are of the type [EditorGUILayout](https://docs.unity3d.com/ScriptReference/EditorGUILayout.html) or [GUILayout](https://docs.unity3d.com/ScriptReference/GUILayout.html).

### Resolution
Use the functions in the [EditorGUI](https://docs.unity3d.com/ScriptReference/EditorGUI.html) or [GUI](https://docs.unity3d.com/ScriptReference/GUI.html) classes, providing the `Rect` position that is a parameter to [OnGUI](https://docs.unity3d.com/ScriptReference/PropertyDrawer.OnGUI.html).  

If a PropertyDrawer requires more height it can be allocated by overriding the [GetPropertyHeight](https://docs.unity3d.com/ScriptReference/PropertyDrawer.GetPropertyHeight.html) function.