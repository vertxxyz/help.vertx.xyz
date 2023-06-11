## Serializing a field

### Fields

Mark your variable with a [`[SerializeField]`](https://docs.unity3d.com/ScriptReference/SerializeField.html) attribute:  
<<Code/Serialization/SerializeField.rtf>>  

or make it `public`:  
<<Code/Serialization/Public.rtf>>

:::warning{.inline}
A `static`, `const`, or `readonly` field cannot be serialized.
:::

### Properties

Unity doesn't serialize properties. You can serialize the **backing field** of an [auto-property](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties) using `[field: SerializeField]`. Versions before 2020 the names did not display appropriately in the Inspector.  
```csharp
[field: SerializeField]
public float Value { get; private set; }
```

:::warning{.inline}
The property must have a `set` accessor, and cannot be `static`.
:::

#### Reasons to avoid serializing backing fields
- The field will be serialized as `<PropertyName>k__BackingField` which adds complexity in editor extensions.
- Refactoring auto-properties into properties and fields occurs quite often, requiring the attribute be moved, and [`FormerlySerializedAs`](https://docs.unity3d.com/ScriptReference/Serialization.FormerlySerializedAsAttribute.html) targeting the annoying name.
- Certain cases can be appear ambiguous when multiple Attributes are targeting the backing field or property which one is being targeted.

---  

[My variable is still not appearing in the inspector.](Serializing%20A%20Field%202.md)