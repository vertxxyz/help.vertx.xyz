## Serializing interfaces
### 2019.3+
You can use the [SerializeReference](https://docs.unity3d.com/ScriptReference/SerializeReference.html) attribute to serialize references to types inheriting from an interface.  
There are caveats to SerializeReference, so reading the docs is worthwhile.  

There is no simple, default way to assign an instance to a serialize reference field. Instead, you will either have to write your own code, or use a custom property drawer like [Vertx.SerializeReferenceDropdown](https://github.com/vertxxyz/Vertx.SerializeReferenceDropdown).

### Pre-2019.3
Interfaces are not serializable.  
Instead, serialize UnityEngine.Object types like [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html) or [MonoBehaviour](https://docs.unity3d.com/Manual/class-MonoBehaviour.html) using inheritance.