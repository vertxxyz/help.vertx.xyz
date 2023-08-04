## Editor persistence

Editors and EditorWindows use the same persistence rules as other UnityEngine.Object types when crossing the Play Mode barrier.  
This means that you can declare serialized variables and they will persist, but other variables will be transient.  

### Resolution
See [serializing a field](../../Serialization/Serializing%20A%20Field%201.md) to learn how to serialize fields, and what types are serializable by Unity.  

:::info
As Editors and EditorWindows are UnityEngine.Object types you can serialize fields into them directly, and they should persist for as long as they are open.
:::

---

See [persisting changes](Persisting%20Changes.md) if your fields are already serialized.