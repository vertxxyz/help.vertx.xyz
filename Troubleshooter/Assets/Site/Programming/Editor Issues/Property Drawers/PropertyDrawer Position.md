### PropertyDrawer Position

#### Description
PropertyDrawers do not support IMGUI layout functionality. You must not use methods that are of the type [EditorGUILayout](https://docs.unity3d.com/ScriptReference/EditorGUILayout.html) or [GUILayout](https://docs.unity3d.com/ScriptReference/GUILayout.html).

#### Resolution
Use the functions in the [EditorGUI](https://docs.unity3d.com/ScriptReference/EditorGUI.html) or [GUI](https://docs.unity3d.com/ScriptReference/GUI.html) classes, and provide the `Rect` position that was a parameter to the [OnGUI](https://docs.unity3d.com/ScriptReference/PropertyDrawer.OnGUI.html) function.  
If a PropertyDrawer requires more height it can be allocated by overriding the [GetPropertyHeight](https://docs.unity3d.com/ScriptReference/PropertyDrawer.GetPropertyHeight.html) function.