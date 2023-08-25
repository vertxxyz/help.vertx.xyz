## Bitmasks and LayerMasks

A bit is a `0` or a `1`. `int` is a 32 bit value, 32 different `0`'s or `1`'s.  
In a bitmask `0` is an inactive layer, and a `1` is active.  
This means we can represent 32 layer toggles with a single `int` value.

The [`LayerMask`](https://docs.unity3d.com/ScriptReference/LayerMask.html) struct describes bitmasks used in by physics
and rendering APIs in Unity.
A `LayerMask` can be exposed in the inspector to easily author masks, and is implicitly converted to `int` for use in
bitmask operations and physics methods.

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

The information below shows how to manually define author and manage masks entirely through code when required.

### Creating masks

To create a bitmask with a single layer
enabled, [shift](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#left-shift-operator-)
a single bit over to the position in the mask that matches the layer index.  
For example, to create a mask with layer **5** active, create `int` with a the first bit enabled, a `1`, then shift that
bit over 5 places to the 6th index (layers are **0 indexed** so this is layer 5).  
It's worth noting that the first bit (the least significant bit) is the rightmost bit, similar to a decimal integer.

```csharp
int mask = 1 << 5;
//    1 : 000001
// mask : 100000
```

### Combining masks

To combine a mask we perform
a [logical or](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-or-operator-).  
If a bit is enabled (`1`) in either one mask **or** the other, it will be in our result.  
This example creates a mask where both layer **5** and **8** are enabled.

```csharp
int mask = (1 << 5) | (1 << 8);
// (1 << 5) : 000100000
// (1 << 8) : 100000000
//     mask : 100100000
```

### Inverting a mask

Bitmasks can be inverted with
the [bitwise complement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#bitwise-complement-operator-)
operator, which will reverse each bit.

```csharp
layerMask = ~layerMask;
//  layerMask : 00000000000000000000000100100000
// ~layerMask : 11111111111111111111111011011111
```

### Remove a layer from a mask

Create an inverted mask using the `~` operator, where the layer to remove is now a `0`, the rest `1`'s. Now perform
a [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-)
to compare each bit and set the resulting bit if both bits are enabled.

```csharp
mask &= ~(1 << 8);
//              mask : 00000000000000000000000100100000
//         ~(1 << 8) : 11111111111111111111111011111111
// mask &= ~(1 << 8) : 00000000000000000000000000100000
```

### Checking if a mask contains a layer

Create a mask with our
layer, [logical and](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-),
if the original mask shared a layer with our new mask the result will not be `0`.

```csharp
if ((mask & (1 << layer)) != 0)
{
    ...
```

---  

General information relating to bitmasks (unrelated to `LayerMask`) can be found [here](../../Info/Bitmasks.md).