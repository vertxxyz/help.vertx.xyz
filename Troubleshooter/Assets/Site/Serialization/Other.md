## Serializing other types
If you are trying to serialize another collection type like:  
- `Dictionary`
- `HashSet`

These types are not supported collections.  

You will need to use another type for serialization, or use [ISerializationCallbackReceiver](https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html) to map to a serializable structure like a `List`.

---

If you are trying to serialize something else, it must follow the rules for [serializing custom types](Custom%20Types.md), if not, it may just not be serializable!