## Serializing a field

### General

Ensure your declared variable is either marked with a `[SerializeField]` attribute:  
<<Code/Serialization/SerializeField.rtf>>  

or is `public`:  
<<Code/Serialization/Public.rtf>>

A `static`, `const`, or `readonly` field **cannot** be serialized.

### Properties

:::warning
Unity will not serialize properties, so make sure your variable is a field type.
:::  

You can serialize a backing field of an auto-property using `[field: SerializeField]`. Pre-2020 this did not display properly in the Inspector.  
The field will be serialized as `<PropertyName>k__BackingField`, which adds complexity when renaming fields and creating editor extensions. Manually serializing a backing field is generally more desirable.

---  

[My variable is still not appearing in the inspector.](Serializing%20A%20Field%202.md)