## Bit masks/Layer Masks
### Description
`int` is a 32 bit value, A bit is 0 or 1; a 0 in a bitmask is an inactive layer, and a 1 is active.  
This means we can represent 32 layer toggles with a single `int` value.

### Creating masks from layers
To create a bitmask with a single layer enabled, [shift](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#left-shift-operator-) a single bit over to the position in the mask that matches the layer index.  
For example, to create a mask with layer `5` active, create `int` with a the first bit enabled, a `1`, then shift that bit over 5 places to the 6th index (layers are **0 indexed** so this is layer 5).  
It's worth noting that the first bit (the least significant bit) is the rightmost bit, similar to a decimal integer.
```csharp
int mask = 1 << 5;
//    1: 000001
// mask: 100000
```

### Declaring masks
Generally Masks are declared as enums so they can be easily referenced by name.
The [Flags Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute) is used to indicate this is a bitmask. The attribute also modifies [ToString](https://docs.microsoft.com/en-us/dotnet/api/system.enum.tostring) to print more relevant values.

Enums are `int` values by default, but any [integral numeric type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types) can be used when defining an enum.  
:::warning
In this example we use a `byte`, as the range is known to be less than 8 layers.
:::  

<<Code/Bitmasks/Declaring.rtf>>

### Combining masks
To combine or add to a mask we perform a [logical or](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-or-operator-), which will compare each bit from each mask and run an or comparison with the results. With a layer in one mask set `true`, and the other `false`, is either `true` or `false` enabled? Yes, so the resulting bit is enabled. Only if both masks have the layer disabled will the result be disabled.
<<Code/Bitmasks/Combining.rtf>>

### Inverting a mask
Bitmasks can be inverted with the [bitwise complement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#bitwise-complement-operator-) operator, which will reverse each bit.
```csharp
layerMask = ~layerMask;
//  layerMask : 00000010000000000000000100100000
// ~layerMask : 11111101111111111111111011011111
```

### Remove from a mask
Create an inverted mask using the `~` operator, where the layer to remove is now a 0, the rest 1's. Now perform a [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-) to compare each bit and only set the resulting bit if both bits are enabled.
<<Code/Bitmasks/Removing.rtf>>

### Toggling mask values
The [logical exclusive or](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-exclusive-or-operator-) (XOR) will not alter values not present in the second operand, while toggling the mask when a match is found.
<<Code/Bitmasks/Toggling.rtf>>

### Checking if a mask contains another mask
```csharp
if ((mask & queryMask) == queryMask) {
```

When dealing with an enum, you could alternatively choose to use the [Enum.HasFlag](https://docs.microsoft.com/en-us/dotnet/api/system.enum.hasflag) method.

---  

Similar information relating to LayerMasks used in Unity's Physics can be found [here](../Programming/Physics/Bitmasks.md).