## Serializing interfaces
### 2019.3+
You can use the [SerializeReference](https://docs.unity3d.com/ScriptReference/SerializeReference.html) attribute to serialize references to types inheriting from an interface.  
There are caveats to SerializeReference, so reading the docs is worthwhile.

### Pre-2019.3
Interfaces are not serializable.  
The fallback is to serialize UnityEngine.Object types like ScriptableObject or MonoBehaviour using inheritance.