## Bit masks and Layer Masks
### Description
`int` is a 32 bit value, A bit is `0` or `1`; a `0` in a bitmask is an inactive layer, and a `1` is active.  
This means we can represent 32 layer toggles with a single `int` value.  

In Unity's case they have a struct called a [LayerMask](https://docs.unity3d.com/ScriptReference/LayerMask.html) used to define bitmasks used in the Physics engine.
A LayerMask can be exposed in the inspector to easily author masks, and is implicitly converted to `int` for use in bitmask operations and physics methods.
The information below shows how to manually define author and manage masks entirely through code when required.  

### Creating masks
To create a bitmask with a single layer enabled, [shift](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#left-shift-operator-) a single bit over to the position in the mask that matches the layer index.  
For example, to create a mask with layer `5` active, create `int` with a the first bit enabled, a `1`, then shift that bit over 5 places to the 6th index (layers are **0 indexed** so this is layer 5).  
It's worth noting that the first bit (the least significant bit) is the rightmost bit, similar to a decimal integer.
```csharp
int mask = 1 << 5;
//    1 : 000001
// mask : 100000
```

### Combining masks
To combine a mask we perform a [logical or](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-or-operator-), which will compare each bit from each mask and run an or comparison with the results. With a layer in one mask set `true`, and the other `false`, is either `true` or `false` enabled? Yes, so the resulting bit is enabled. Only if both masks have the layer disabled will the result be disabled.  
This example creates a mask where both layer 5 and 8 are enabled.
```csharp
int mask = (1 << 5) | (1 << 8);
// (1 << 5) : 000100000
// (1 << 8) : 100000000
//     mask : 100100000
```

### Inverting a mask
Bitmasks can be inverted with the [bitwise complement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#bitwise-complement-operator-) operator, which will reverse each bit.
```csharp
layerMask = ~layerMask;
//  layerMask : 00000000000000000000000100100000
// ~layerMask : 11111111111111111111111011011111
```

### Remove a layer from a mask
Create an inverted mask using the `~` operator, where the layer to remove is now a 0, the rest 1's. Now perform a [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-) to compare each bit and set the resulting bit if both bits are enabled.
```csharp
mask &= ~(1 << 8);
//              mask : 00000000000000000000000100100000
//         ~(1 << 8) : 11111111111111111111111011111111
// mask &= ~(1 << 8) : 00000000000000000000000000100000
```

### Checking if a mask contains a layer
Create a mask with our layer, [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-), if the original mask shared a layer with our new mask the result will not be 0.
```csharp
if ((mask & (1 << layer)) != 0) {
```

---
[My raycast is still not returning a result](../Raycasting/Incorrect%20Parameters.md)