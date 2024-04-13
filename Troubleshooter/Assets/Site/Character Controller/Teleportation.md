# Character Controller teleportation
To teleport a [Character Controller](https://docs.unity3d.com/Manual/class-CharacterController.html) it must be disabled.

### Example
```csharp
// Disable the CharacterController.
_characterController.enabled = false;
// Move it.
_characterController.transform.position = _target.position;
// Re-enable it.
_characterController.enabled = true;
```

If you were previously [referencing](../References.md) a `Transform`, reference the `CharacterController` instead so you can toggle `enabled`.

---

If you still have issues teleporting, call [`Physics.SyncTransforms`](https://docs.unity3d.com/ScriptReference/Physics.SyncTransforms.html) after teleportation to apply transform changes.
