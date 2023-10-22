## Serialized defaults

The value a serialized variable is set to in code is only the default when a new instance is created from scratch.  
After the value has been serialized, that value will override anything that it's initialised to code.

### Resolution
#### I want to keep the value serialized so I can modify it in the Inspector
Set the value to the variable via the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html).  
The value in code is only a default for new instances. You can also right-click on a component header and **Reset** it to its default values.

#### I want the value to only be set in code
Either make the variable `private` or `protected`, or mark it with the [`[NonSerialized]`](https://learn.microsoft.com/en-us/dotnet/api/system.nonserializedattribute) attribute so it is not serialized by Unity.  
Generally it's a good idea to keep parameters serialized so you can tweak the values without editing code and recompiling.