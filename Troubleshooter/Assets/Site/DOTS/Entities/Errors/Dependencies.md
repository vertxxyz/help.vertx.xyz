# Dependencies

Reading and writing from native collections requires maintaining a delicate chain of dependencies.  
Systems take dependencies in, and output them. Collections should not be accessed without relying on those dependencies.

## Resolution

:::warning  
Other exceptions may lead to dependency errors, so be careful to resolve the easiest ones first!  
Errors thrown immediately *after* this error can be the root cause.  
:::

### Utilise JobHandles
Feed the system's handle (`state.Dependency`) into the first thing that requires something complete before it runs.  
Whatever receives a JobHandle should return its own, and you create a chain of these dependencies which must ultimately be set back to the system's handle.  

### Verify dependencies in value types are modified
If you are passing around a `SystemState`, make sure it's passed by `ref` so modifications to its dependency actually update across systems.

### Do not read from collections before dependencies are evaluated
Ensure you don't use any properties of a native collection (like `.Length`) before you have *waited* on a dependency. In the case of an [`EntityQuery`](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.EntityQuery.html), you should use [`CalculateEntityCount`](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.EntityQuery.CalculateEntityCount.html#Unity_Entities_EntityQuery_CalculateEntityCount) to get the length of an array without requiring a dependency.

In some cases you can use a job interface like [`IJobParallelForDefer`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Jobs.IJobParallelForDefer.html), which does not rely on `Length` being provided before execution.
