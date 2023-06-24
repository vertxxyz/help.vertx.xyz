## Simple dependency injection
### Description
Dependency injection (DI) might sound complex, but it's simply the process of having a reference passed to an object.  

### Implementation
::::note   
#### 1. Create an initializer method
The method should receive the references your class requires as parameters, which are then assigned locally.

```csharp
private Example _example1;
private Example _example2;

public void Initialise(Example example2, Example example2)
{
    _example1 = example1;
    _example2 = example2;
}
```

:::info{.inline}
If you aren't using a `UnityEngine.Object` subtype you can use contructors and variable initializers, and don't need to make an Initialise method.
:::  
::::  
::::note  
#### 2. Where your class is created or instanced, reference your objects
See [serialized references](Serialized%20References.md), or use another method like `GetComponent`.
```csharp
[Header("References")]
[SerializeField] private Example _example1;
[SerializeField] private Example _example2;
```
::::  
::::note  
#### 3. Call your initializer method with the references as arguments
Where your class is created or instanced call the method on the new instance.
```csharp
public void Spawn()
{
    var instance = Instantiate(_prefab);
    instance.Initialise(_example1, _example2);
}
```
::::  

Now your instance has references to those objects without having directly referenced them itself.

### Notes
#### Awake
`Awake` is called as soon as an object is created, so your instance will not be initialised yet. Don't attempt to use the references at this point.
#### Object pooling
This is a common pattern when used with object pooling, as you can call your initializer method again and reset the instance's configuration, allowing it to be reused anew.
#### Third-party frameworks
There are third-party DI frameworks that attempt to automate complex chains of dependencies, but you should learn the basics before assessing whether they suit you, they aren't for everyone!