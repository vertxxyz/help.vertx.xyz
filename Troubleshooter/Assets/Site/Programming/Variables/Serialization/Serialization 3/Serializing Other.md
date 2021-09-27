## Serializing Other Types
If you are trying to serialize another Collection type like:  
- Dictionary
- HashSet

These types are not supported collections.  

You will need to use another type for serialization, or use an [ISerializationCallbackReceiver](https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html)

---

If you are trying to serialize something else, it must follow the rules for [serializing custom types](Serializing%20Custom%20Types.md), if not, it may just not be serializable!