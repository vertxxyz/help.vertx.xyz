## Serializing custom types
### I am trying to serialize a built-in type
Various other non-primitive types provided by Unity are serializable.  
Common examples of these include: `UnityEvent`, `Bounds`,   
Serializable unity types should appear in [debug mode](Debug%20Mode.md); look for examples of them appearing in built-in components.

### I am trying to serialize my own type
1. Custom structs and classes should be marked with the `[System.Serializable]` attribute.
1. ::collapse::{.collapse}
::::
Ensure that there are serializable types in that structure:  
<<Serialization/Serializable Types.md>>  
marked with the `[SerializeField]` attribute, or are `public`.  
:::info{.inline}
Empty structures or those without serializable fields are also not serializable.
:::
::::
1. Don't mark your classes, structs, or fields with `abstract`, `static`, `const`, or `readonly`.  

---

- [My class is generic.](Generic%20Types.md)
- [I am serializing an interface.](Interfaces.md)
- [I am directly nesting collections.](Nested%20Collections.md)
