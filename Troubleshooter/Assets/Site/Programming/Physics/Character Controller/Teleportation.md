## Character Controller teleportation
To teleport a [Character Controller](https://docs.unity3d.com/Manual/class-CharacterController.html) it must be disabled.  

#### Example
```csharp
// Disable the CharacterController
characterController.enabled = false;
// Move it         
characterController.transform.position = target.position;
// Re-enable it
characterController.enabled = true;
```

If you were previously [referencing](../../References.md) a `Transform`, you will need to update your code to reference the `CharacterController` instead so you can toggle `enabled`.

---

If you are still having issues teleporting a Character Controller try calling [`Physics.SyncTransforms`](https://docs.unity3d.com/ScriptReference/Physics.SyncTransforms.html) to apply transform changes after teleportation.