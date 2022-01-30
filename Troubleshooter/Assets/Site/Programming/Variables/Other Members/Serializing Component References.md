## Serializing Component references
### Description
To refer to members (eg. variables and methods) of a component, you must define which instance is being used.  
Creating a serialized field allows that instance to be referred to elsewhere in the class.

### Resolution

The field must be marked with [SerializeField](https://docs.unity3d.com/ScriptReference/SerializeField.html):

<<Code/Variables/Serialized Reference.rtf>>

**or** can be `public`:  
<<Code/Variables/Public Reference.rtf>>

:::note
The example uses the `Transform` type, it will need to be replaced with the type intended to be referenced.
:::

The instance the variable is pointing to is defined in the Inspector.  
Dragging a GameObject from the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) into the field will reference the first matching Component found on the object.

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/inspector-references.webm"></video>  
:::error{.small .img-note-wide}
Drag a **Component** into the slot or you will get a [NullReferenceException](../../Common%20Errors/Runtime%20Exceptions/NullReferenceException.md).
:::

### Notes
When the Component or ScriptableObject is created Unity will deserialize the reference into the field, there is no need to further assign the value in code.  
Serialized references are locally maintained when Objects are instanced; references in prefab instances will be local, not referring to the original.  

<<Variables/Further Info.md>>