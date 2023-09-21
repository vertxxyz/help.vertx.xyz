## Serialization depth limit
Unity serializes C# types by value. This means that even with [reference types](../Programming/Value%20And%20Reference%20Types.md#reference-types) it will not serialize `null`.
Value types in C# cannot have circular references (references to themselves within themselves), and the same goes for by value serialization. Unity cannot prevent serializable classes referencing themselves, so to prevent an infinite loop it implements a serialization depth limit.

### Resolution
Choose a resolution:
- Remove the reference loop.
- Use UnityEngine.Object types.
- Use SerializeReference.

::::note
#### Remove the reference loop
Avoid having a serialized class reference itself, or referencing other classes in a way that cause a loop.
::::

::::note
#### Use UnityEngine.Object types
You can use [`ScriptableObject`](https://docs.unity3d.com/Manual/class-ScriptableObject.html) or [`MonoBehaviour`](https://docs.unity3d.com/Manual/class-MonoBehaviour.html) references instead of plain C# classes to prevent a loop.
Unlike plain C# classes, references to UnityEngine.Object subtypes are by reference, and are null/none by default.
::::

::::note
#### Use SerializeReference
You can use [`[SerializeReference]`](https://docs.unity3d.com/ScriptReference/SerializeReference.html) to serialize by reference instead of by value.
SerializeReference is selective about what it can serialize, so be sure to read the manual. It also doesn't show an editor by default, and this may be incompatible with your setup.

I have a package, [Vertx.SerializeReferenceDropdown](https://github.com/vertxxyz/Vertx.SerializeReferenceDropdown), that adds a type selection dropdown for serialize reference fields.
::::
