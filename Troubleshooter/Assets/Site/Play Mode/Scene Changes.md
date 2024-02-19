## Scene changes in Play Mode
In Play Mode, any changes you make are temporary, and are reset when you exit Play Mode.  

You can change the editor's Play Mode tint color in **Edit | Preferences | Colors | Play Mode Tint** to make it more obvious if you find you often mistakenly make temporary changes.

### Persisting changes
#### Copying changes from singular components or fields
You can right-click on a component header and select **Copy Component**, exit Play Mode, and apply the copied values via **Paste Component Values**.
Some properties also support copying their values individually.

#### Using assets
Assets like [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html) persist their changes across Play Mode. This can cause issues, and as such the general advice is that ScriptableObjects should be treated as immutable data structures. You can use this information as you will!

#### Using prefabs
You can [create a prefab](https://docs.unity3d.com/Manual/CreatingPrefabs.html) out of an object at runtime and the prefab will persist. This is helpful to capture changes for an entire object, though it's a very crude operation.
