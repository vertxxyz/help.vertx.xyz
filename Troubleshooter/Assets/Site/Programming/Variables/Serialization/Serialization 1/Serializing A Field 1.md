### Serializing a Field

Ensure your declared variable is either marked with a `[SerializeField]` attribute:  
<<Code/Serialization/SerializeField.rtf>>  

or is `public`:  
<<Code/Serialization/Public.rtf>>  

Unity will not serialize properties, so make sure your variable is a field type.

A `static`, `const`, or `readonly` field **cannot** be serialized.

[My variable is still not appearing in the inspector](Serialization%202/Serializing%20A%20Field%202.md)