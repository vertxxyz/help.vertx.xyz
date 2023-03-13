## Static Entities
### Description
Gameobjects marked with [static flags](https://docs.unity3d.com/Manual/StaticObjects.html) or have [`StaticOptimizeEntity`](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.StaticOptimizeEntity.html) applied to them will override any `TransformUsageFlags` previously applied to the object.

### Resolution
*Any* static flags will override TransformUsageFlags applied to the object. If `StaticOptimizeEntity` is applied, the hierarchy will be frozen.  
Either remove these, or post-process your objects in a system to reapply their transforms.