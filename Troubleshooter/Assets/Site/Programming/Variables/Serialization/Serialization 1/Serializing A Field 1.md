## Serializing a Field

Ensure your declared variable is either marked with a `[SerializeField]` attribute:  
<<Code/Serialization/SerializeField.rtf>>  

or is `public`:  
<<Code/Serialization/Public.rtf>>

A `static`, `const`, or `readonly` field **cannot** be serialized.

---  

:::warning
Unity will not serialize properties, so make sure your variable is a field type.
:::  

You can serialize a backing field of an auto-property using `[field: SerializeField]`. Pre-2020 this did not display properly in the Inspector and could cause serialization issues.

---  

[My variable is still not appearing in the inspector](../Serialization%202/Serializing%20A%20Field%202.md)