<<Abbreviations/IDE.md>>
*[spawner]: A component in the scene that spawns your prefab asset.
*[spawner's]: A component in the scene that spawns your prefab asset.
*[instance]: A single GameObject or Component. A component can exist in the scene many times, one of these is an instance. In this case, the instance is a copy of your prefab that exists in the scene.
## Referencing Prefabs from Scenes

You can only reference components on the root (the top-level GameObject) of a prefab.
See [references to prefab children](References%20To%20Prefab%20Children.md) if you want to reference prefab sub-objects.

### Implementation

::::note
#### 1. Add or choose a root component on your prefab
On the top-level GameObject of your prefab asset, [add](https://docs.unity3d.com/Manual/UsingComponents.html) or choose a component that you want to reference.
:::error{.small}
This example uses `PrefabComponentType`, it will need to be replaced with the root component type you chose.
:::

Your spawner will clone the whole prefab and you will end up with a reference to the copy of the root component.

::::

::::note
#### 2. On your spawner, expose a serialized reference to your root component
The [field](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields) must be marked with [`SerializeField`](https://docs.unity3d.com/ScriptReference/SerializeField.html):
```csharp
[SerializeField] private PrefabComponentType _prefab;
```

**or** can be `public`:
```csharp
public PrefabComponentType Prefab;
```
::::

::::note
#### 3. Reference the prefab asset in the spawner's Inspector

^^^
<video width="750" height="200" autoplay loop muted controls><source type="video/webm" src="https://unity.huh.how/Video/prefab-references.webm"></video>
^^^ ::Drag the prefab asset into the slot or you will get an [`UnassignedReferenceException`](../Runtime%20Exceptions/UnassignedReferenceException.md).::{.error}

Dragging a prefab from the [Project window](https://docs.unity3d.com/Manual/ProjectView.html) into the field will reference the first matching Component found on the root of the object.

::::

::::note
#### 4. Instance the prefab asset from your spawner
Create a variable for your new instance, and assign it the result of calling [`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) with the prefab you declared earlier.
```csharp
// A reference to our in-Scene clone of the prefab.
private PrefabComponentType _instance;

void Spawn()
{
    // Clone the prefab and assign it to our variable.
    _instance = Instantiate(_prefab);
}
```
::::

::::note
#### 5. Access the member you care about through your new instance
`public` members on the root component can be accessed via the instance.
```csharp
// Variables and properties
var variable = _instance.Variable;
_instance.Variable = variable;

// Methods
_instance.Method();
```
:::info{.small}
If you don't have autocomplete, [configure your IDE](../IDE%20Configuration.md) to easily find member names and get error highlighting.
:::
::::

### Notes
#### Serialization and instancing
When cloning a component using [`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html), all attached objects and components are cloned with their properties set like those of the original object.
Serialized references are locally maintained when Objects are instanced; references in prefab instances will be local, not referring to the original. Without indirection it's not possible for a prefab instance to maintain a reference to the prefab it was spawned from.

#### Prefabs and runtime
Prefabs are an editor-only concept. At runtime prefab instances are not logically connected to what spawned them, and nor is any information about prefab hierarchies or overrides maintained.

#### Prefab roots
The prefab root is the top-level GameObject of a prefab. You cannot directly reference the children of a prefab from the scene, so prefab children should generally reference a component on the root if they want data you've configured from the scene.
Also see [references to prefab children](References%20To%20Prefab%20Children.md) to learn more.

#### Referencing Components, not GameObjects
Don't reference a `GameObject` unless you only use its methods. Referencing a component directly avoids using `GetComponent`.

```csharp
/* --- 🟠 Wasteful and error-prone --- */
[SerializeField] private GameObject _prefab;

void Spawn()
{
    GameObject instance = Instantiate(_prefab);
    // GetComponent is required to do anything beyond calling SetActive.
    instance.GetComponent<Example>().Foo();
}

/* --- 🟢 Simple and communicates usage --- */
[SerializeField] private Example _prefab;

void Spawn()
{
    Example instance = Instantiate(_prefab);
    // Has direct access to useful methods.
    instance.Foo();
}
```
