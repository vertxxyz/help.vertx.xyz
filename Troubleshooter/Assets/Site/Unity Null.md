<<Abbreviations/NRE.md>>
## Unity null
Comparing [`UnityEngine.Object`](https://docs.unity3d.com/ScriptReference/Object.html) derived types to `null` in Unity may not work how you expect.

### Details
#### General

The equality operators (`==` and `!=`) for `UnityEngine.Object` types have been overridden by Unity, and don't only perform reference comparisons.
A Unity `Object` is split into two parts: managed C#, and native C++. Users interact with the managed object, and the engine manages the native object.
When an `Object` is compared with `null` and it isn't *really* `null`, a check against the native object occurs. This makes it possible to [`Destroy`](https://docs.unity3d.com/ScriptReference/Object.Destroy.html) an `Object` somewhere, and have an entirely different reference evaluate to `null`.

#### Editor-only
The editor adds additional context through this destroyed or faked object which is raised when exceptions occur.
- When something isn't assigned in the editor, an [`UnassignedReferenceException`](Runtime%20Exceptions/UnassignedReferenceException.md) is thrown.
- When there used to be an object, a [`MissingReferenceException`](Runtime%20Exceptions/MissingReferenceException.md) is thrown.

This context includes the location that is missing the object, which wouldn't be provided with a normal NRE.

### Ramifications
#### Modern null-checking operators
The null conditional (`?.`) and null coalescing (`??`, `??=`) operators, and `is` null checks (`is null`, `is not null`, `is {}`) will not function correctly with `UnityEngine.Object` types as these operators could not be overridden by Unity. When Unity was created these operators did not exist, and this legacy remains rooted in examples and APIs.

Avoid using these operators with UnityEngine Objects, and instead consider alternatives. For example:

```csharp
// 🔴 Incorrect
_component = GetComponent<Example>() ?? gameObject.AddComponent<Example>();
// 🟢 Correct
if (!TryGetComponent(out _component))
    _component = gameObject.AddComponent<Example>();
```

#### Garbage collection and allocation
If you want to reclaim memory on Destroyed objects you need to additionally ensure all references are set to `null` to allow the C# garbage collector to reclaim that memory exactly like other heap allocated types.

[`GetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) will allocate memory in the Editor when nothing is found because it returns a fake object, using [`TryGetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html) will not.

#### Access
Usually, an NRE is thrown whenever a `null` object is accessed, but seeing as a Unity Object can be fake-null, members that do not perform lifetime checks can still be accessed and function in this state.

#### Newing Unity objects
If you create a Unity Object via the `new` operator, this will subtly fail in the majority of cases (there are valid situations like `GameObject`), where the native portion of the object was never created, and so you have an improperly initialised Unity-null object.

#### Using interfaces or `object`
Testing destroyed or missing Unity Objects that are casted as interfaces or `object` will fail to work. They will use the default object equality.
Either consider an alternative that safely checks for null, or cast to `UnityEngine.Object`.

```csharp
IExample example = GetComponent<IExample>();
// 🔴 Incorrect, this will fail to detect destroyed or missing objects.
if (example != null) { }
// 🟢 Correct
if ((UnityEngine.Object)example != null) { }
// 🟢 Correct
if (TryGetComponent<IExample>(out example)) { }
```

#### Using generics
Unconstrained generic types won't use the Unity Object equality.

```csharp
// 🔴 Incorrect, this will fail to detect destroyed or missing objects.
public class NullTest<T>
{
    public bool IsNull { get; }
    
    public NullTest(T value) => IsNull = value == null;
}

// 🟢 Correct
public class NullTest<T> where T : UnityEngine.Object
{
    public bool IsNull { get; }
    
    public NullTest(T value) => IsNull = value == null;
}
```

### Read more
- 2014 Unity blog: ["Custom == operator, should we keep it?"](https://blog.unity.com/technology/custom-operator-should-we-keep-it)
- Resharper/Rider suggestion: ["Possible unintended bypass of lifetime check of underlying Unity engine object"](https://github.com/JetBrains/resharper-unity/wiki/Possible-unintended-bypass-of-lifetime-check-of-underlying-Unity-engine-object).
