## Prefabs referencing in-Scene Components
### Description
:::warning
Assets cannot directly refer to Objects in Scenes
:::

Prefabs cannot refer to components in the Scene, though their instances can be configured to.  

### Resolution
The in-scene component that instances your prefab can reference other in-Scene components.
[Create a serialized reference](Serializing%20Component%20References.md) to the target component and pass it to the newly instanced prefab.  

#### Example
<<Code/Variables/Instancing Prefabs.html>>

:::note
**Replace** `PrefabType` with **a component on the root of your prefab** that you wish to configure.  
**Replace** `TargetType` with the component `PrefabType` is looking to be configured with.  
**Replace** `Data` with a property or variable matching `TargetType` on `PrefabType`.  

Ideally a setup method would be used instead of setting fields or properties directly.
:::