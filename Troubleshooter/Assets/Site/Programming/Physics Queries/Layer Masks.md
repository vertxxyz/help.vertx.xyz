## Physics queries: LayerMasks

Layer masks are [bitmasks](../Physics/Bitmasks.md) that describe which layers are active or otherwise.  
:::info{.small}
Active layers will be hit by the query.
:::  
A layer mask may be an `int`, but it doesn't represent a single layer.

:::note
#### 🔴 Incorrect
```csharp
int layerMask = 5;
```

#### 🔴 Incorrect
```csharp
int layerMask = LayerMask.NameToLayer("Name");
```
:::

### Resolution
Correctly create a layer mask:

::::note
#### 🟢 Serialize a `LayerMask`
If bitmasks are confusing, a simple option is to [expose](../Serialization/Serializing%20A%20Field%201.md) a [`LayerMask`](https://docs.unity3d.com/ScriptReference/LayerMask.html) and configure it via the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html).
```csharp
public LayerMask ExampleMask;
```
`LayerMask` can be passed to physics functions as it's implicitly convertible to `int`.  

:::warning{.small}  
Double-check the mask value set in the inspector.  
:::  
::::  
**Or**  
::::note
#### 🟢 Use `LayerMask.GetMask`
Initialise and use a mask created using [`LayerMask.GetMask`](https://docs.unity3d.com/ScriptReference/LayerMask.GetMask.html).  
::::  
**Or**  
::::note
#### 🟢 Use bit shifting
Manually create a mask from layer indices using [bit shifting](../Physics/Bitmasks.md#creating-masks).  
::::  
Then pass that mask to the correct parameter of the query.


---
[I am still having problems with my query.](GameObject%20Layers.md)