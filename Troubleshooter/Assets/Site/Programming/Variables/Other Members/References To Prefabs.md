## Referencing Prefabs from Scenes
#### 1. Expose a serialized reference to the target component
The target component is a component on the **root** of the prefab asset.  
::::note
The field must be marked with [SerializeField](https://docs.unity3d.com/ScriptReference/SerializeField.html):  
<<Code/Variables/Prefab Reference.rtf>>

**or** can be `public`:  
<<Code/Variables/Public Prefab Reference.rtf>>  

:::info{.inline}
This example uses `PrefabComponentType`, it will need to be replaced with the target type.
:::  
::::

#### 2. Reference the instance in the Inspector
::::note
Drag the prefab asset into the slot or you will get an [`UnassignedReferenceException`](../../Common%20Errors/Runtime%20Exceptions/UnassignedReferenceException.md).  

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/prefab-references.webm"></video>
::::

#### 3. Access the member you care about
::::note
`public` members on the target component can be accessed via the instance.
```csharp
// Variables and properties
var variable = _instance.Variable;
_instance.Variable = variable;

// Methods
_instance.Method();
```
If you don't have autocomplete, [configure your IDE](../../IDE%20Configuration.md) to easily find member names and get error highlighting.
::::
### Notes
When cloning a component using [`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html), all attached objects and components are cloned with their properties set like those of the original object.  
<<Variables/Further Info.md>>