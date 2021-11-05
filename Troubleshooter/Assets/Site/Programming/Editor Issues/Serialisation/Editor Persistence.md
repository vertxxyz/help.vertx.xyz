## Editor persistence
### Description
Editors and EditorWindows use the same persistence rules as other UnityEngine.Object types when crossing the Play Mode barrier.  
This means that you can declare serialized variables and they will persist, but other variables will be transient.  

### Resolution
See [serializing a field](../../Variables/Serialization/Serialization%201/Serializing%20A%20Field%201.md) to learn how to serialize fields, and what types are serializable by Unity.  