## Serializing Component references
<<Variables/Serialized References.md>>  
Dragging a GameObject from the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) into the field will reference the first matching Component found on the object.

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/inspector-references.webm"></video>  
:::error{.small .img-note-wide}
Drag a **Component** into the slot or you will get a [Null Reference Exception](../../Common%20Errors/Runtime%20Exceptions/NullReferenceException.md).
:::  
When the Component or ScriptableObject is created Unity will deserialize the reference into the field, there is no need to further assign the value in code.  
In prefabs, serialized references are locally maintained when they are instanced.  

<<Variables/Further Info.md>>