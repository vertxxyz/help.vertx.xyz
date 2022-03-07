## Layer Masks with raycasts
### Description
Layer masks are bitmasks that describe which layers are active or otherwise.  
:::info{.inline}
Active layers will be hit by the raycast.
:::  
A layer mask may be an `int`, but it isn't representing a single layer.  
### Resolution
If bitmasks are confusing, a simple resolution is to expose a `LayerMask` property which can be simply configured via the Inspector.  
```csharp
public LayerMask exampleMask;
```
`LayerMask` is implicitly convertible to `int`, and is simply passed to the `Raycast` functions.  

[I would like to learn more about bitmasks.](../Physics/Bitmasks.md)

---
[I am still having problems with my raycast.](Incorrect%20Parameters.md)