## Serializing nested collections
Unity cannot directly serialize nested, 2D, or jagged, collections.  

### Resolution
#### Nested serialization
Instead of directly serializing a collection inside of another, create a serializable type that you can nest your collection inside.
```csharp
// 🔴 Will not serialize
public List<float[]> Values;

// 🟢 Will serialize
[System.Serializable]
public class ValuesWrapper
{
    public float[] Values;
}

public List<ValuesWrapper> Values;
```

I have a [`JaggedArray<T>`](https://gist.github.com/vertxxyz/8f6e2ec0922c257f6173f331fc6d3370) implementation you are free to use that cleans up the editor for use.

#### Flattened serialization
You can use [`ISerializationCallbackReceiver`](https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html) to manually serialize your nested collection into a flattened (1 dimensional) collection.
Note that you would need to write a Property Drawer if you wanted to display it in a nested view in the inspector.

