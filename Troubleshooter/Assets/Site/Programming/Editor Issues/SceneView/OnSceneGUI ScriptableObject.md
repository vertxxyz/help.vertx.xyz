## OnSceneGUI not called: Scriptable Object
### Description
[Editor.OnSceneGUI](https://docs.unity3d.com/ScriptReference/Editor.OnSceneGUI.html) is not called for editors of ScriptableObjects.

### Resolution
Subscribe to the [duringSceneGui](https://docs.unity3d.com/ScriptReference/SceneView-duringSceneGui.html) callback (formerly onSceneGUIDelegate).  

<<Code/Editor/DuringSceneGUI.rtf>>

---  

<<Editor/OnSceneGuiCallback.md>>

---  

[OnSceneGUI is still not called.](OnSceneGUI%20Gizmos.md)