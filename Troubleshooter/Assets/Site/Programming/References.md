## Referring to members in other scripts

There are various ways to create a reference between one object and another:

### Serialized references
Serialized references are exposed via the inspector and are defined per-instance. They are fast and configurable.  
[Learn more.](References/Serialized%20References.md)

### Singletons
Singletons are referenced in code and require **only one instance** of the target type.  
[Learn more.](References/Singletons.md)

### GetComponent APIs
Prefer serialized references for their speed and configurability. [`GetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html), [`TryGetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html),
[`GetComponentInChildren`](https://docs.unity3d.com/ScriptReference/Component.GetComponentInChildren.html), and other similar methods are perfect for dynamic runtime references, like those resolved during collisions and scene queries.

### Find APIs
Avoid the various find methods unless you are debugging or prototyping. These methods are generally extremely slow, and even when used from `Awake` or `Start` can cause hitches during loading. Prefer [serialized references](References/Serialized%20References.md) or [singletons](References/Singletons.md).  
If you are referencing objects you spawned at runtime, add them to a collection (like a List) when they are created, and reference that object to get your instances instead.

### Alternate methods
Other ways to refer to external members include dependency injection frameworks and varied use of the `static` keyword. Generally these can be avoided until users are familiar with more basic and common concepts.