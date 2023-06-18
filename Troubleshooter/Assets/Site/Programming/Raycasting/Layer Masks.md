## LayerMasks with raycasts
### Description
Layer masks are **bitmasks** that describe which layers are active or otherwise.  
:::info{.inline}
Active layers will be hit by the raycast.
:::  
A layer mask may be an `int`, but it doesn't represent a single layer.  

:::note  
#### 🔴 Incorrect
```csharp
int layerMask = 5;
```
:::

### Resolution
Correctly create a layer mask:

:::note  
#### 🟢 Serialize a `LayerMask`
If bitmasks are confusing, a simple option is to [expose](../Serialization/Serializing%20A%20Field%201.md) a [`LayerMask`](https://docs.unity3d.com/ScriptReference/LayerMask.html) and configure it via the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html).  
```csharp
public LayerMask ExampleMask;
```
`LayerMask` can be passed to physics functions as it's implicitly convertible to `int`.  
:::  
**Or**  
:::note  
#### 🟢 Use `LayerMask.GetMask`
Initialise and use a mask created using [`LayerMask.GetMask`](https://docs.unity3d.com/ScriptReference/LayerMask.GetMask.html).  
:::  
**Or**  
:::note  
#### 🟢 Use bit shifting
Manually create a mask from layer indices using [bit shifting](../Physics/Bitmasks.md#creating-masks).  
:::  
Then pass that mask to the correct parameter of the raycast.  


---
- [I would like to learn more about bitmasks.](../Physics/Bitmasks.md)
- [I am still having problems with my raycast.](Incorrect%20Parameters.md)