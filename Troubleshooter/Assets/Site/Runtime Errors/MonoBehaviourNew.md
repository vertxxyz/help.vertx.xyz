# MonoBehaviour construction
```
You are trying to create a MonoBehaviour using the 'new' keyword.
This is not allowed.
MonoBehaviours can only be added using AddComponent().
Alternatively, your script can inherit from ScriptableObject or no base class at all
UnityEngine.MonoBehaviour:.ctor()
```


[`MonoBehaviour`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html) is the user-facing class used for creating Components.
Components are required to be attached to a `GameObject`.
Unity's API does not support using constructors (`new`) to create MonoBehaviours, this is done internally in the native side of the engine.  

```diff
// Invalid way to construct a MonoBehaviour.
- new Example();
```

Objects created in this way will evaluate to `null` while still existing on the C# side. See [Unity null](../Unity%20Null.md).

## Resolution
::::note  
### Continuing to use a MonoBehaviour
Instead of creating a `MonoBehaviour` using the `new` keyword you instead need to use the [`AddComponent`](https://docs.unity3d.com/ScriptReference/GameObject.AddComponent.html) API.
If you cannot find the line of code that is causing the problem it will be explicitly mentioned in the stack trace returned with the error.

```csharp
var example = gameObject.AddComponent<Example>();
```

To emulate a constructor, you can call a separately defined method.

```csharp
example.Initialize(configuration);
```  
Note that the component's [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) will be called before the function is ran.  
::::  
::::note  
### Using a plain class
Instead of inheriting from `MonoBehaviour` you can create a class that doesn't!
A plain class lives on its own and is garbage collected when it's no longer referenced. You may also be able to [serialize](../Serialization/Custom%20Types.md) it with the inspector.  
::::
