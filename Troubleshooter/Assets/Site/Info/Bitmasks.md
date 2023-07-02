## Bitmasks

A bit is a `0` or a `1`. `int` is a 32 bit value, 32 different `0`'s or `1`'s.  
In a bitmask `0` is an inactive layer, and a `1` is active.  
This means we can represent 32 layer toggles with a single `int` value.

### Creating masks from layers
To create a bitmask with a single layer enabled, [shift](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#left-shift-operator-) a single bit over to the position in the mask that matches the layer index.  
For example, to create a mask with layer **5** active, create `int` with a the first bit enabled, a `1`, then shift that bit over 5 places to the 6th index (layers are **0 indexed** so this is layer 5).  
It's worth noting that the first bit (the least significant bit) is the rightmost bit, similar to normal numbers.
```csharp
int mask = 1 << 5;
//    1: 000001
// mask: 100000
```

### Declaring masks
Generally Masks are declared as enums so they can be easily referenced by name.
The [Flags Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute) is used to indicate this is a bitmask. The attribute also modifies [`ToString`](https://docs.microsoft.com/en-us/dotnet/api/system.enum.tostring) to print more relevant values.

Enums are `int` values by default, but any [integral numeric type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types) can be used when defining an enum.  
:::warning
In this example we use a `byte`, as the range is known to be less than 8 layers.
:::  

<<Code/Bitmasks/Declaring.rtf>>

### Combining masks
To combine or add to a mask we perform a [logical or](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-or-operator-).  
If a bit is enabled (`1`) in either one mask **or** the other, it will be in our result.

<<Code/Bitmasks/Combining.rtf>>

### Inverting a mask
Bitmasks can be inverted with the [bitwise complement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#bitwise-complement-operator-) operator, which will reverse each bit.
```csharp
mask = ~mask;
//  mask : 00000010000000000000000100100000
// ~mask : 11111101111111111111111011011111
```

### Remove from a mask
Create an inverted mask using the `~` operator, where the layer to remove is now a 0, the rest 1's. Now perform a [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-) to compare each bit and only set the resulting bit if both bits are enabled.
<<Code/Bitmasks/Removing.rtf>>

### Toggling mask values
The [logical exclusive or](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-exclusive-or-operator-) (XOR) will not alter values not present in the second operand, while toggling the mask when a match is found.
<<Code/Bitmasks/Toggling.rtf>>

### Checking if a mask contains another mask
```csharp
if ((mask & queryMask) == queryMask)
{
    // mask contains queryMask.
}
```

When dealing with an enum, you could alternatively choose to use the [`Enum.HasFlag`](https://docs.microsoft.com/en-us/dotnet/api/system.enum.hasflag) method, note that this may box.

### A mask with all values enabled
[Invert](#inverting-a-mask) `0` to enable all bits.
```csharp
var enabledMask = ~0;
```
Note that this value may not be the same as "Everything" or "All", and different implementations may or may not interpret it that way. Note how `Days.Everyday` is `01111111`, not all bits are enabled. Your implementations may need to consider this.

### Setting a mask using another mask
If `setOn` is `true` then `~mask` will invert the original mask, `& Days.Weekend` isolates the bits we want to enable, giving us a mask that only contains the weekend bits that aren't enabled in the original `mask`. We then [toggle](#toggling-mask-values) the mask with this value.
```csharp
var weekendCorrectedMask = mask ^ ((setOn ? ~mask : mask) & Days.Weekend);
//                 mask : 01001001  setOn: true
//                ~mask : 10110110
//       & Days.Weekend : 00100000
// weekendCorrectedMask : 01101001
```
If `setOn` is `false` then `mask` does nothing, `& Days.Weekend` isolates the bits we want to disable, giving us a mask which contains the weekend bits that are enabled in the original `mask`. We then [toggle](#toggling-mask-values) the mask with this value.
```csharp
var weekendCorrectedMask = mask ^ ((setOn ? ~mask : mask) & Days.Weekend);
//                 mask : 01001001  setOn: false
//       & Days.Weekend : 01000000
// weekendCorrectedMask : 00001001
```

---  

Similar information relating to LayerMasks used in Unity's Physics can be found [here](../Programming/Physics/Bitmasks.md).