### Layer Masks with Raycasts
#### Description
Layer masks are bitmasks that describe which layers are active or otherwise.  
A layer mask may appear to be an `int`, but it is not representing a single layer.  
#### Resolution
If bitmasks are confusing, a simple resolution is to expose a `LayerMask` property which can be simply configured via the Inspector.  
```csharp
public LayerMask exampleMask;
```
`LayerMask` is implicitly convertible to `int`, and is simply passed to the `Raycast` functions.

---
[I would like to learn more about bitmasks](Bitmasks.md)  
[I am still not returning a result](Incorrect%20Parameters.md)