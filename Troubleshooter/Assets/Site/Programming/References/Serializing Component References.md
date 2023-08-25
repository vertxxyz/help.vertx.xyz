<<Abbreviations/IDE.md>>
*[instance]: A single GameObject or Component. A component can exist in the scene many times, one of these is an instance.
*[target component]: A component in the scene that you want to reference.
## Serializing Component references

To refer to members (such as variables and methods) of a component, you must define which instance is being used.  
Creating a serialized field allows that instance to be referred to elsewhere in the class.

:::warning
You cannot serialize references between scenes. See [cross-scene references](Cross-Scene%20References.md) to learn more.
:::

If you are trying to reference a component in a dynamic situation, consider the [GetComponent methods](GetComponent%20Methods.md).

### Implementation
::::note  
#### 1. Expose a serialized reference to the target component  
The [field](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields) must be marked with [`SerializeField`](https://docs.unity3d.com/ScriptReference/SerializeField.html):

<<Code/Variables/Serialized Reference.html>>

**or** can be `public`:  
<<Code/Variables/Public Reference.html>>

:::info{.inline}
This example uses the `Transform` type, it will need to be replaced with the target type.
:::

::::  
::::note  
#### 2. Reference the target component in the Inspector  
Do not directly reference the script asset. The target component must be an instance [added to an object in the scene](https://docs.unity3d.com/Manual/UsingComponents.html).  

^^^
<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://unity.huh.how/Video/inspector-references.webm"></video>  
^^^ ::Drag a **Component** into the slot or you will get an [`UnassignedReferenceException`](../Common%20Errors/Runtime%20Exceptions/UnassignedReferenceException.md).::{.error}

Dragging a GameObject from the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) into the field will reference the first matching Component found on the object.

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
:::info{.inline}  
If you don't have autocomplete, [configure your IDE](../IDE%20Configuration.md) to easily find member names and get error highlighting.
:::  

The usage must be at a [method or block level scope](../Other/Scopes.md).
::::

### Notes
#### Dragging components and using multiple inspectors
You can drag the header of components themselves. When the origin and destination are on two separate objects you may need to open two inspectors to perform the drag. You can right-click on the tab of an editor window and select **Add Tab | Inspector** to create a second one. You can also select the small lock icon in the top right to lock it to its current selection.

#### Serialization and instancing
When the Component or ScriptableObject is created Unity will deserialize the reference into the field, there is no need to further assign the value in code.  
Serialized references are locally maintained when Objects are instanced; references in prefab instances will be local, not referring to the original.  

#### Referencing Components, not GameObjects
Don't reference a `GameObject` unless you only use its methods. Referencing a component directly avoids using `GetComponent`.

```csharp
// 🟠 Wasteful and error-prone.
[SerializeField] private GameObject _example;

void CallFoo()
{
    // GetComponent is required to do anything beyond calling SetActive.
    _example.GetComponent<Example>().Foo();
}

// 🟢 Simple and communicates usage.
[SerializeField] private Example _example;

void CallFoo()
{
    // Has direct access to useful methods.
    _example.Foo();
}
```