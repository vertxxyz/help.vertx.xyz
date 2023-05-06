## Layer Masks with raycasts
### Description
Layer masks are **bitmasks** that describe which layers are active or otherwise.  
:::info{.inline}
Active layers will be hit by the raycast.
:::  
A layer mask may be an `int`, but it isn't representing a single layer.  
### Resolution
Correctly create a layer mask:

:::note  
#### Serialize a `LayerMask`
If bitmasks are confusing, a simple option is to expose a `LayerMask` field which can be simply configured via the Inspector.  
```csharp
public LayerMask MaskExample;
```
`LayerMask` is implicitly convertible to `int`, and is simply passed to the `Raycast` functions.  
:::  
**Or**  
:::note  
#### Use `LayerMask.GetMask`
Initialise and use a mask created using [`LayerMask.GetMask`](https://docs.unity3d.com/ScriptReference/LayerMask.GetMask.html).  
:::  
**Or**  
:::note  
#### Use bit shifting
Use bit shifting to manually create a mask using layer indexes.  
:::  
[I would like to learn more about bitmasks.](../Physics/Bitmasks.md)

---
[I am still having problems with my raycast.](Incorrect%20Parameters.md)