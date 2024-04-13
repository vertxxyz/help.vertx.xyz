# Persisting changes

If you're modifying something from an editor context it needs to be dirtied.  
If something is changed and not dirtied, that change is transient and will be wiped.

## Options in order of preference:
::::note
### 1. SerializedObject and SerializedProperty
:::note
Although daunting at first, [`SerializedObject`](https://docs.unity3d.com/ScriptReference/SerializedObject.html) is a powerful system to record changes and automatically display extendable UI through property drawers. To learn how to access serialized fields and sub-objects, see [SerializedObject: how-to](SerializedObject%20How-to.md).
:::

Call [`ApplyModifiedProperties`](https://docs.unity3d.com/ScriptReference/SerializedObject.ApplyModifiedProperties.html) **after** making property modifications to apply persistent changes.  

This will also record an undo; if you don't want an undo, use [`ApplyModifiedPropertiesWithoutUndo`](https://docs.unity3d.com/ScriptReference/SerializedObject.ApplyModifiedPropertiesWithoutUndo.html).  
If your Object can be modified externally, you should call [`Update`](https://docs.unity3d.com/ScriptReference/SerializedObject.Update.html) before drawing the fields to synchronise the SerializedObject with the base object.  
::::
::::note
### 2. Undo.RecordObject
Call [`RecordObject`](https://docs.unity3d.com/ScriptReference/Undo.RecordObject.html) **before** directly making changes to an Object to apply persistent changes[^1].  

To correctly handle prefab changes, call [`RecordPrefabInstancePropertyModifications`](https://docs.unity3d.com/ScriptReference/PrefabUtility.RecordPrefabInstancePropertyModifications.html) after `RecordObject`.
::::
::::note
### 3. EditorUtility.SetDirty
The final option for recording modifications is [`SetDirty`](https://docs.unity3d.com/ScriptReference/EditorUtility.SetDirty.html). This will not record undo states.
::::

---  

If you're still not seeing changes persist, ensure that the variables are actually [serializable](../../Serialization.md).

[^1]: The diff is run at the end of the frame. So if you're using this in combination with [`SaveOpenScenes`](https://docs.unity3d.com/ScriptReference/SceneManagement.EditorSceneManager.SaveOpenScenes.html) in one frame, the scene will not be marked as dirty, and it will not be saved. 
If you have this issue, you can use [`delayCall`](https://docs.unity3d.com/ScriptReference/EditorApplication-delayCall.html) to save the Scene a little later, or manually [mark the scenes as dirty](https://docs.unity3d.com/ScriptReference/SceneManagement.EditorSceneManager.MarkAllScenesDirty.html).
