## Serializing simple types in collections
Simple collections of serializable types should serialize correctly.  
#### Arrays and Lists
Arrays (`float[]`), and Lists (`List<float>`), are the two supported collection types.  

:::warning
Multi-dimensional and jagged arrays are not serializable. See [**other collection types**](#other-collection-types) below.
:::

#### Dictionaries

Dictionaries are not serializable, but you can implement [ISerializationCallbackReceiver](https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html) to manually serialize a dictionary into serializable datastructures. There's an example of that structure in the documentation.  
There are also SerializableDictionary implementations out there that are wrapper classes that implement the interface, and may also have a custom property drawer.

#### Other collection types
You can implement [ISerializationCallbackReceiver](https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html) to manually serialize these types, but it can add unnecessary complexity to your codebase.