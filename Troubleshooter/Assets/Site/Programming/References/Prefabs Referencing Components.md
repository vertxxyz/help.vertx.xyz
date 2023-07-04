## Prefabs referencing in-Scene Components

:::warning
Assets cannot directly refer to Objects in Scenes
:::

Prefab assets cannot refer to components in the Scene, their instances can be configured with those references.  

### Resolution
The in-scene component that instances your prefab can reference other in-Scene components, so have it pass your instance that reference.

::::note  
#### Follow these steps to configure a prefab instance with a reference:
Refer to the table to understand the terms used in the following steps.

| Name             | Description                                                    |
|------------------|----------------------------------------------------------------|
| Spawner          | A component in the scene that spawns your prefab.              |
| Root component   | A component on the top-level GameObject of your prefab.        |
| Target component | A component in the scene that you want to pass to your prefab. |

1. Create references on your spawner:
   1. [Reference the root component (your prefab).](References%20To%20Prefabs.md)
   1. [Reference the target component.](Serializing%20Component%20References.md)
1. [`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) your prefab.  
  This creates a new instance of the prefab in the scene, and returns the copy of the root component.
1. Create members on your root component to pass your target component to.
1. Assign the target component to your new root component instance.  

::::
### Example
#### Spawner component
```csharp
using UnityEngine;

/// <summary>
/// This component lives in the scene and spawns instances of MoveToTargetExample.
/// </summary>
public class SpawnerExample : MonoBehaviour
{
   // (1. 1) Our prefab:
   [SerializeField] private MoveToTargetExample _prefab; 
   // (1. 2) Our target component:
   [SerializeField] private Transform _target;
   
   public void SpawnAndConfigurePrefab()
   {
       // (2) Instantiate the prefab:
       MoveToTargetExample instance = Instantiate(_prefab);
       // (4) Configure the newly-created instance with the in-Scene reference:
       instance.Initialise(_target);
   }
}
```

#### Prefab root component
```csharp
using UnityEngine;

/// <summary>
/// This component lives on the root of a prefab, and is configused by SpawnerExample after it is instanced.
/// </summary>
public class MoveToTargetExample : MonoBehaviour
{
    // (3) A local reference to the target:
    private Transform _target;
    
    // Step (4) calls this method:
    public void Initialise(Transform target)
    {
        // (4) Configure this instance with the reference to target that was passed from the spawner.
        _target = target;
    }
    
    /* Collapsable: Example usage */
    // --- Example usage ---
    [SerializeField] private float _speed = 1;
    
    public void Update() =>
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    /* End Collapsable */
}
```

:::note  
You must change the types and variables used in this example to match yours:
- `Transform` must be replaced with your target type.  
   This is the component from the scene that your prefab cares about.
- `MoveToTargetExample` must be replaced with a type of component on the prefab root.  
   This is the component on the top-level GameObject that you will be configuring.

You can choose to set members directly, or customise your `Initialise` method to take more parameters.  
:::

### Notes
#### Awake
`Awake` is called as soon as an object is created; in this case, don't attempt to use the references in `Awake` as they haven't been assigned by the spawner.

---

If you are confused by any individual steps, click through the links in the resolution to see detailed examples.