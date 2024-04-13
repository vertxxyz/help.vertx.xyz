# Coroutines: Inactive objects
```
Coroutine couldn't be started because the the game object 'X' is inactive!
```

Coroutines can only be run on active GameObjects.

## Resolution
::::note
### Check that the GameObject is active before you start the coroutine
Use [`activeInHierarchy`](https://docs.unity3d.com/ScriptReference/GameObject-activeInHierarchy.html) to check whether the GameObject is active. Don't use [`activeSelf`](https://docs.unity3d.com/ScriptReference/GameObject-activeSelf.html), the GameObject's active state depends on the hierarchy.

```csharp
if (gameObject.activeInHierarchy)
{
    StartCoroutine(MyCoroutine());
}
```
::::
Or
::::note
### Start the coroutine on another object that is active
[Reference](../References/Serialized%20References.md) another component that is in an active hierarchy and start the coroutine on it instead.
```csharp
_otherComponent.StartCoroutine(MyCoroutine());
```
::::

---

- [Return to Coroutines.](../Coroutines.md)
