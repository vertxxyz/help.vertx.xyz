## Dependencies
### Description
Reading and writing from native collections requires maintaining a delicate chain of dependencies.  
Systems take dependencies in, and output them. Collections should not be accessed without relying on those dependencies.

### Resolution
Feed the system's handle (`state.Dependency`) into the first thing that requires something has completed before it runs.  
Whatever receives a JobHandle should return its own, and you create a chain of these dependencies which must ultimately be set back to the system's handle.  

Ensure you don't use any properties of a native collection (like `.Length`) before you have *waited* on a dependency. In the case of an [EntityQuery](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.EntityQuery.html), you should use [`CalculateEntityCount`](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.EntityQuery.CalculateEntityCount.html#Unity_Entities_EntityQuery_CalculateEntityCount) to get the length of an array without requiring a dependency.  

In the rare cases you are manually managing EntityQuery dependencies, it has the functions `GetDependency` and `SetDependency`.

:::warning
Other exceptions may lead to dependency errors, so be careful to resolve the easiest ones first!
:::