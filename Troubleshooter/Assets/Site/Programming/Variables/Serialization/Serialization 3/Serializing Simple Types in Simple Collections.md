### Serializing Simple Types in Collections
Simple collections of serializable types should serialize correctly.  
Arrays (eg. `float[]`,) and Lists (eg. `List<float>`,) are the two supported collection types.  

---

Dictionaries are not serializable, but you can implement [ISerializationCallbackReceiver](https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html) to manually reserialize a dictionary at edit-time. There's an example of that structure in the documentation.  
There are also SerializableDictionary implementations out there that are wrapper classes that implement the interface, and may also have a custom property drawer.