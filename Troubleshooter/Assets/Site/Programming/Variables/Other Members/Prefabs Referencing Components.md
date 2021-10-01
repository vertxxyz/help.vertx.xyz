## Prefabs Referencing In-Scene Components
### Description
:::warning
Assets cannot directly refer to Objects in Scenes
:::

Prefabs cannot refer to components in the Scene, though their instances can be configured to.  

### Resolution
The component that instances the prefab should be present in the scene, and can then be used to reference other in-Scene components.  
Wherever the prefab is being instanced, [create a serialized reference](Serializing%20Component%20References.md) to the target component and pass it to the newly instanced prefab.  

#### Example[^1]
<<Code/Variables/Instancing Prefabs.html>>

[^1]:`PrefabType` should be replaced with the root component type on your prefab you wish to configure.  
`ConfigType` should be replaced with the type `PrefabType` is looking to be configured with.  
Ideally a setup method would be used instead of setting fields or properties directly.  