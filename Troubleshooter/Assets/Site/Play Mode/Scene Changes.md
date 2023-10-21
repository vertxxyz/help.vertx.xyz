## Scene changes in Play mode
In Play mode, any changes you make are temporary, and are reset when you exit Play mode.  

You can change the editor's Play mode tint color in **Edit | Preferences | Colors | Play mode Tint** to make it more obvious if you find you often mistakenly make temporary changes.

### Persisting changes
#### Copying changes from singular components or fields
You can right-click on a component header and select **Copy Component**, exit Play mode, and apply the copied values via **Paste Component Values**.
Some properties also support copying their values individually.

#### Using assets
Assets like [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html) persist their changes across Play mode. This can cause issues, and as such the general advice is that ScriptableObjects should be treated as immutable data structures. You can use this information as you will!

#### Using prefabs
You can [create a prefab](https://docs.unity3d.com/Manual/CreatingPrefabs.html) out of an object at runtime and the prefab will persist. This is helpful to capture changes for an entire object, though it's a very crude operation.