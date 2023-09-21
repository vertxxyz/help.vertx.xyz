## SerializedProperty.objectReferenceValue
```
type is not a supported pptr value
```


The [objectReferenceValue](https://docs.unity3d.com/ScriptReference/SerializedProperty-objectReferenceValue.html) property is used to access `UnityEngine.Object` types from SerializedProperties.  
If you are seeing this error it is certain that the property value does not inherit from `UnityEngine.Object`.

### Resolution
Debug `propertyType` to determine what the type of the SerializedProperty is, and alter the code accordingly.

---  

[I would like to learn more about Serialized Properties.](../../Editor%20Extensions/Serialisation/SerializedObject%20How-to.md)