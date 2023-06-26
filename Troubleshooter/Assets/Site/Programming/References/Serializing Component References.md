<<Abbreviations/IDE.md>>
## Serializing Component references

To refer to members (such as variables and methods) of a component, you must define which instance is being used.  
Creating a serialized field allows that instance to be referred to elsewhere in the class.

:::warning
You cannot serialize references between scenes. [See here to learn more.](Cross-Scene%20References.md)
:::

### Implementation

::::note  
#### 1. Expose a serialized reference to the target component  
The field must be marked with [`SerializeField`](https://docs.unity3d.com/ScriptReference/SerializeField.html):

<<Code/Variables/Serialized Reference.html>>

**or** can be `public`:  
<<Code/Variables/Public Reference.html>>

:::info{.inline}
This example uses the `Transform` type, it will need to be replaced with the target type.
:::
::::  
::::note  
#### 2. Reference the instance in the Inspector  
Create an instance by [adding the Component](https://docs.unity3d.com/Manual/UsingComponents.html) to an object in the scene, and drag it into the exposed field.  
Dragging a GameObject from the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) into the field will reference the first matching Component found on the object.

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://unity.huh.how/Video/inspector-references.webm"></video>  
:::error{.small .img-note-wide}
Drag a **Component** into the slot or you will get a [NullReferenceException](../Common%20Errors/Runtime%20Exceptions/NullReferenceException.md).
:::
::::

::::note  
#### 3. Access the member you care about
`public` members on the target component can be accessed via the instance.
```csharp
// Variables and properties
var variable = _target.Variable;
_target.Variable = variable;

// Methods
_target.Method();
```
If you don't have autocomplete, [configure your IDE](../IDE%20Configuration.md) to easily find member names and get error highlighting.
::::
### Notes
#### Dragging components and using multiple inspectors
You can drag the header of components themselves. When the origin and destination are on two separate objects you may need to open two inspectors to perform the drag. You can right-click on the tab of an editor window and select **Add Tab | Inspector** to create a second one. You can also select the small lock icon in the top right to lock it to its current selection.

#### Serialization and instancing
When the Component or ScriptableObject is created Unity will deserialize the reference into the field, there is no need to further assign the value in code.  
Serialized references are locally maintained when Objects are instanced; references in prefab instances will be local, not referring to the original.  

#### Referencing Components, not GameOjects
<<Variables/Further Info.md>>