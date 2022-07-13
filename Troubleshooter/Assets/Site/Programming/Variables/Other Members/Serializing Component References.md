<<Abbreviations/IDE.md>>
## Serializing Component references
### Description
To refer to members (such as variables and methods) of a component, you must define which instance is being used.  
Creating a serialized field allows that instance to be referred to elsewhere in the class.

### Resolution

#### 1. Expose a serialized reference to the target component  
::::note
The field must be marked with [SerializeField](https://docs.unity3d.com/ScriptReference/SerializeField.html):

<<Code/Variables/Serialized Reference.html>>

**or** can be `public`:  
<<Code/Variables/Public Reference.html>>

:::info{.inline}
This example uses the `Transform` type, it will need to be replaced with the target type.
:::
::::
#### 2. Reference the instance in the Inspector  
::::note
Create an instance by [adding the Component](https://docs.unity3d.com/Manual/UsingComponents.html) to an object in the scene, and drag it into the exposed field.  
Dragging a GameObject from the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) into the field will reference the first matching Component found on the object.

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/inspector-references.webm"></video>  
:::error{.small .img-note-wide}
Drag a **Component** into the slot or you will get a [NullReferenceException](../../Common%20Errors/Runtime%20Exceptions/NullReferenceException.md).
:::
::::

#### 3. Access the member you care about
::::note
`public` members on the target component can be accessed via the instance.
```csharp
// Variables and properties
var variable = _target.Variable;
_target.Variable = variable;

// Methods
_target.Method();
```
If you don't have autocomplete, [configure your IDE](../../IDE%20Configuration.md) to easily find member names and get error highlighting.
::::
### Notes
When the Component or ScriptableObject is created Unity will deserialize the reference into the field, there is no need to further assign the value in code.  
Serialized references are locally maintained when Objects are instanced; references in prefab instances will be local, not referring to the original.  

<<Variables/Further Info.md>>