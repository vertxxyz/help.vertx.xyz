## Serializing Custom Types

Ensure **all** custom structs and classes in your serialization hierarchy are marked with the `[System.Serializable]` attribute.

Ensure that the types contained in your structure that you wish to be serialized are also serializable types:

<<Serialization/Serializable Types.md>>  

and are also either marked with the `[SerializeField]` attribute, or are `public`.

----

Your classes/structs must **not** be `abstract`, `static`, or `readonly`.  
Empty structures or those without serializable fields are also not serializable.

[My class is generic](../Serialization%204/Serializing%20Generic%20Types.md)  
[I am serializing an interface](../Serialization%204/Serializing%20Interfaces.md)