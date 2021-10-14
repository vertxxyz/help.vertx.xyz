## Persisting changes
If you're trying to set something from an editor context it needs to be dirtied.  
:::info
If something is changed and not dirtied, that change is transient and will be wiped.
:::

The options are **in order of preference**:

### SerializedObject / SerializedProperty
**After** making property modifications the Serialized Object needs changes applied to persist them to the base object using [ApplyModifiedProperties](https://docs.unity3d.com/ScriptReference/SerializedObject.ApplyModifiedProperties.html).  
Applying changes will also record a modification for the Undo stack. If you do not want an Undo, use [ApplyModifiedPropertiesWithoutUndo](https://docs.unity3d.com/ScriptReference/SerializedObject.ApplyModifiedPropertiesWithoutUndo.html).  
If your Object can be modified externally, you should call [Update](https://docs.unity3d.com/ScriptReference/SerializedObject.Update.html) before drawing the fields to synchronise the SerializedObject with the base object.

### Undo.RecordObject
**Before** making changes to an Object directly you need to call
[RecordObject](https://docs.unity3d.com/ScriptReference/Undo.RecordObject.html).  
If there are changes, this will both dirty the Object, and record an Undo to the stack.  
This can be done  
To correctly handle prefab changes, calling [RecordPrefabInstancePropertyModifications](https://docs.unity3d.com/ScriptReference/PrefabUtility.RecordPrefabInstancePropertyModifications.html) after RecordObject is advised.  

The dirty state will only be set when the diff is run at the end of the frame. So if you're using this in combination with SaveAllScenes in one frame, the scene will not be marked as dirty, and it will not be saved.  
If you have this issue, you can use [delayCall](https://docs.unity3d.com/ScriptReference/EditorApplication-delayCall.html) to save the Scene a little later, or to manually [mark the scenes as dirty](https://docs.unity3d.com/ScriptReference/SceneManagement.EditorSceneManager.MarkAllScenesDirty.html).


###  EditorUtility.SetDirty
The final option for recording modifications is [SetDirty](https://docs.unity3d.com/ScriptReference/EditorUtility.SetDirty.html). This will not record Undo states.

---  

If you're still not seeing changes persist, ensure that the variables are actually [serializable](../../Variables/Serialization%20First.md).