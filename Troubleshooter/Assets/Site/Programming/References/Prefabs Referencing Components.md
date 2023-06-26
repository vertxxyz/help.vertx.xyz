## Prefabs referencing in-Scene Components

:::warning
Assets cannot directly refer to Objects in Scenes
:::

Prefab assets cannot refer to components in the Scene, their instances can be configured with those references.  

### Resolution
The in-scene component that instances your prefab (the "spawner") can reference other in-Scene components.  

#### Follow these steps to configure a prefab instance with a reference:

1. Create references **on your spawner**:
   1. [Reference a component on the root of your **prefab**.](References%20To%20Prefabs.md)
   1. [Reference the **in-scene component** you want to assign to your prefab](Serializing%20Component%20References.md).
1. [`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) your **prefab**, this returns a new **instance** in the scene of the root component you referenced.
1. Assign the **in-scene component** to your new **instance**.

#### Example
<<Code/Variables/Instancing Prefabs.html>>

:::note  
You must change the types and variables used in this example to match yours:
- `Transform` must be replaced with your target type.
- `PrefabType` must be replaced with a type of component on the root of your prefab.  
- `TargetTransform` must be replaced by a variable or method that takes your target type.  
:::