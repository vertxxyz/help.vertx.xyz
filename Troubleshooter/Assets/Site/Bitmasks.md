## Bitmasks and LayerMasks
### Bitmasks
A bitmask is a representation of many states using a single value.

A bit is a `0` or a `1`. In a bitmask `0` is an inactive layer, and a `1` is active.  
As `int` is a 32 bit value, it can be used to represent 32 toggleable layers.

### LayerMasks
The [`LayerMask`](https://docs.unity3d.com/ScriptReference/LayerMask.html) struct describes a bitmask of [layers](https://docs.unity3d.com/Manual/Layers.html) used by physics and rendering APIs in Unity.
It's implicitly converted to `int` when using physics methods.

:::info{.small}
Because a `LayerMask` is a bitmask, it shouldn't be treated as a single layer.
:::

:::::note{#layermask-diagram}
#### Interactive diagram
<script type="module" src="/Scripts/Interactive/Bitmasks/layerMaskDropdown.js?v=1.0.1"></script>
::::{.inspector-root}
:::{.control-root}
<div class="control-label">Bitmask</div>
<div class="control-element bitmask" id="layermask-bitmask">
<span class="bitmask__bit-container">0</span>
</div>
:::
:::{.control-root}
<div class="control-label">LayerMask</div>
<div class="control-element control-dropdown" id="layermask-dropdown">
  <label class="control-dropdown__label" tabindex="0">Select</label>

  <div class="control-dropdown__list">
    <label class="control-dropdown__option" tabindex="-1">
      <input type="checkbox" name="control-dropdown__checkbox" value="Example" tabindex="-1" />
      Unintitialised
    </label>
  </div>
</div>
:::
::::

:::note{.center}
Select the bits and interact with the dropdown to see their relationship.
:::
:::::

### Creating masks
To create a bitmask with a specific layer enabled, [shift](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#left-shift-operator-) a single bit over to the position that matches the layer's index.  

:::note  
To create a mask with layer **5** active:
1. Create `int` with the first bit enabled, a `1`.
2. Shift that bit over **5** places to the 6th index (layers are 0 indexed so this is layer 5).

^^^
```csharp
int mask = 1 << 5;
//    1 : 000001
// mask : 100000
```
^^^::Note that the first bit (the least significant bit) is the rightmost bit, similar to a decimal integer.::{.info}
:::

A `LayerMask` can be [serialized](Serialization/Serializing%20A%20Field%201.md) to easily author masks in the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html) based on Unity's [layers](https://docs.unity3d.com/Manual/Layers.html).  
:::note
```csharp
[SerializeField] private LayerMask _mask;
```
**Or**
```csharp
public LayerMask Mask;
```
:::

You can also create a LayerMask in code with [`LayerMask.GetMask`](https://docs.unity3d.com/ScriptReference/LayerMask.GetMask.html).

### Declaring custom masks
Generally masks are declared as an [enum](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum) so they can be easily referenced and manipulated by name.  
The [`Flags` attribute](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute) indicates the enum is a bitmask, and modifies [`ToString`](https://docs.microsoft.com/en-us/dotnet/api/system.enum.tostring) to return more relevant values.

Enums are `int` values by default, but any [integral numeric type](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types) can be used when defining an enum.

:::note
#### Example

^^^
<<Code/Bitmasks/Declaring.rtf>>
^^^ ::We could use an underlying type of `byte`, as the range is known to be less than 8 layers. Optimisations like these may decrease performance due to memory alignment.::{.info}

:::

### Combining masks

To combine a mask we perform
a [logical or](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-or-operator-).
If a bit is enabled (`1`) in either one mask **or** the other, it will be in our result.

#### Examples

:::note
Creating a mask where both layer **5** and **8** are enabled.

```csharp
int mask = (1 << 5) | (1 << 8);
// (1 << 5) : 000100000
// (1 << 8) : 100000000
//     mask : 100100000
```
:::

:::note
Creating a mask that includes Monday and the weekend.  
<<Code/Bitmasks/Combining.rtf>>
:::

### Inverting a mask

Bitmasks can be inverted with
the [bitwise complement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#bitwise-complement-operator-)
operator, which will reverse each bit.

:::note
```csharp
layerMask = ~layerMask;
//  layerMask : 00000000000000000000000100100000
// ~layerMask : 11111111111111111111111011011111
```
:::

### Removing from a mask

1. Create an inverted mask using the `~` operator, where the layer to remove is now a `0`, the rest `1`'s.
1. Perform a [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-). If a bit is enabled (`1`) in one mask **and** the other, it will be in our result.  
   Because the layer is a `0`, it will not be in both masks, and is removed.

#### Examples
:::note
Removing layer **8** from a mask.
```csharp
mask &= ~(1 << 8);
//              mask : 00000000000000000000000100100000
//         ~(1 << 8) : 11111111111111111111111011111111
// mask &= ~(1 << 8) : 00000000000000000000000000100000
```
:::
:::note
Removing Wednesday from a mask.
<<Code/Bitmasks/Removing.rtf>>
:::

### Checking if a mask...
#### Contains a layer (contains any)
If a mask contains a layer, it **and** a mask describing that layer mustn't be 0.
1. [Create a mask](#creating-masks) with our layer.
1. Perform a [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-).
1. Compare the result with `0`.

:::note
```csharp
if ((mask & (1 << layer)) != 0)
{
    // mask contains layer.
}
```
:::

This logic can be used to conservatively check if values are in a mask. 

#### Contains another mask (contains all)
If a mask contains all the values of another mask, the values in it **and** the other mask must be the mask you expect.
1. Perform a [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-).
1. Compare the result with the mask you expect.

:::note
```csharp
if ((mask & queryMask) == queryMask)
{
    // mask contains queryMask.
}
```
:::

When dealing with an enum, you could alternatively choose to use the [`Enum.HasFlag`](https://docs.microsoft.com/en-us/dotnet/api/system.enum.hasflag) method, note that this may [box](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing).

### Toggling mask values
The [logical exclusive or](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-exclusive-or-operator-) (XOR) will not alter values not present in the second operand, while toggling the mask when a match is found.

#### Examples
:::note
Toggling layer **8** (in this case, off).
```csharp
mask ^= 1 << 8;
//           mask : 00000000000000000000000100100000
//         1 << 8 : 00000000000000000000000100000000
// mask ^= 1 << 8 : 00000000000000000000000000100000
```
:::

:::note
Toggling Wednesday (in this case, off).  
<<Code/Bitmasks/Toggling.rtf>>
:::

### Creating a mask with all values enabled
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

## Logic tables
:::::{.grid-container #main-page--content .tables--centered}  
::::{.grid-item}  

### `&` • Logical AND operator

| A & B | 0   | 1   |
|-------|-----|-----|
| **0** | N 0 | N 0 |
| **1** | N 0 | Y 1 |

::::  
::::{.grid-item}


### `|` • Logical OR operator


| A \| B | 0   | 1   |
|--------|-----|-----|
| **0**  | N 0 | Y 1 |
| **1**  | Y 1 | Y 1 |

::::  
::::{.grid-item}

### `^` • Logical exclusive OR operator

| A ^ B | 0   | 1   |
|-------|-----|-----|
| **0** | N 0 | Y 1 |
| **1** | Y 1 | N 0 |


::::  
:::::  
