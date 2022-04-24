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

:::info
Unity will not serialize properties.
:::  

You can serialize the **backing field** of an [auto-property](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties) using `[field: SerializeField]`. Pre-2020 this did not display properly in the Inspector.  
```csharp
[field: SerializeField]
public float Value { get; private set; }
```

:::warning{.inline}
The property must have a `set` accessor, and cannot be `static`.
:::

The field will be serialized as `<PropertyName>k__BackingField`, which adds complexity when renaming fields or creating editor extensions. Manually serializing a backing field is generally more desirable.

---  

[My variable is still not appearing in the inspector.](Serializing%20A%20Field%202.md)