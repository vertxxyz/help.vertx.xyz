## [Quaternion: Multiplication](https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html)
### Declaration
```csharp
Quaternion * Quaternion
Quaternion * Vector3
```

### Description
Multiplication rotates an orientation or point, combining them in sequence. `A * B`, `A` rotates `B`.

### Usage: Rotating vectors

Local space directions can be multiplied by a world space orientation to produce their world space counterpart.  
This is how [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) `up`, `right`, and `forward` work:  

<<Code/Info/Quaternions/Multiplication 3.html>>  

#### Interactive diagram

:::note{.center}
Drag the axes to change **A**, drag the sphere to modify **B**.  
Press <kbd>X</kbd> to alternate between local and world space handles.
:::

::: {#multiplication-directions .interactive-content}
::: {#multiplication-directions-reset-button .interactive-button}
Reset
:::
:::
<script type="module" src="/Scripts/Interactive/Quaternions/multiplication-directions.js"></script>
<<Code/Info/Quaternions/Multiplication 2.html>>

The logic can be applied to rotate any point around its origin.  

#### Rotate around
[`RotateAround`](https://docs.unity3d.com/ScriptReference/Transform.RotateAround.html) combines vector arithmetic and [`AngleAxis`](AngleAxis.md) with multiplication, here is an annotated version of the logic:  

<<Code/Info/Quaternions/Multiplication 4.html>>  

### Usage: Rotating quaternions
Quaternion multiplication isn't commutative, `A * B != B * A`.  
Rotating `A` influences `B` as a parent rotation.  
Rotating `B` influences `A` as a local space modification.  
To form an intuition about the influence of `A` or `B` on the result, interact with the diagram below.

#### Interactive diagram

:::note{.center}
Drag the axes to interact with A or B, press <kbd>X</kbd> to alternate between local and world space handles.
:::

::: {#multiplication .interactive-content}
::: {#multiplication-reset-button .interactive-button}
Reset
:::
:::
<script type="module" src="/Scripts/Interactive/Quaternions/multiplication.js"></script>

```csharp
transform.rotation = A.rotation * B.rotation;
```

---
Return to [Quaternions.](../Quaternions.md)  
Next, [Quaternion.Inverse.](Inverse.md)