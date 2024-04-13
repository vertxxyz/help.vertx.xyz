# Editor persistence

Editors and EditorWindows use the same persistence rules as other UnityEngine.Object types when crossing the Play Mode barrier.  
This means that you can declare serialized variables and they will persist, but other variables will be transient.  

## Resolution
### My fields are not serialized
See [serializing a field](../../Serialization/Serializing%20A%20Field%201.md) to learn how to serialize fields, and what types are serializable by Unity.  

:::info
As `Editor` and `EditorWindow` are `UnityEngine.Object` types you can serialize fields into them, and they will persist while they're open.
:::

### My fields are already serialized.

See [persisting changes](Persisting%20Changes.md) to learn how to correctly modify serialized variables.
