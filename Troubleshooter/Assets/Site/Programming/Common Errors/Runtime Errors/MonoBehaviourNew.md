## MonoBehaviour construction
```
You are trying to create a MonoBehaviour using the 'new' keyword.
This is not allowed.
MonoBehaviours can only be added using AddComponent().
Alternatively, your script can inherit from ScriptableObject or no base class at all
UnityEngine.MonoBehaviour:.ctor()
```  

### Description
`MonoBehaviour` is the user-facing class used for creating Components.  
Components are required to be attached to a `GameObject`.   
Unity's API does not support using constructors (`new`) to create MonoBehaviours, this is done internally in the native side of the engine.

### Resolution
Instead of creating a `MonoBehaviour` using the `new` keyword you instead need to use the [AddComponent](https://docs.unity3d.com/ScriptReference/GameObject.AddComponent.html) API.  
If you cannot find the line of code that is causing the problem it will be explicitly mentioned in the stack trace returned with the error.  

<<Code/RuntimeErrors/AddComponent.rtf>>