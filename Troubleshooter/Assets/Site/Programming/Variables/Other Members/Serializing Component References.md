### Serializing Component References
<<Variables/Serialized References.md>>
Ensure you have dragged a **Component** into the slot or you will get a [Null Reference Exception](../../Common%20Errors/Runtime%20Exceptions/Null%20Reference%20Exception.md) (Unassigned Reference Exception).  
You can also drag the entire GameObject from the hierarchy into the slot and the relevant Component will be referenced.

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/inspector-references.webm"></video>  

When the Component or ScriptableObject is created Unity will deserialize the reference into the field, there is no need to further assign the value in code. Serialized references are maintained locally to prefabs when they are instanced.  

<<Variables/Further Info.md>>