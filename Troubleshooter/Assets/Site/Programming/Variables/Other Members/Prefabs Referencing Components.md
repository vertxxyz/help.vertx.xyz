## Prefabs Referencing In-Scene Components
:::info
Assets cannot directly refer to Objects in scenes
:::

If you want a prefab to refer to components in the scene, create a serialized reference to that component and pass it to the newly instanced prefab wherever you are instancing it.  

#### Example
<<Code/Variables/Instancing Prefabs.rtf>>  

`PrefabType` should be replaced with the root component type on your prefab you wish to configure, and `ConfigType` should be replaced with the type `PrefabType` is looking
 to be configured with.