## [Quaternion - Multiplication](https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html)
### Declaration
```csharp
Quaternion * Quaternion
Quaternion * Vector3
```

### Description
Multiplication will rotate an orientation or point. `A * B`, `A` rotates `B`.

### Usage - Rotating Points and Directions

To find a world space direction you can multiply its local space counterpart by the world space orientation.  
This is how [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) `up`, `right`, and `forward` works:  

<<Code/Info/Quaternions/Multiplication 3.html>>  

#### Interactive Diagram

::: {#multiplication-directions .interactive-content}
::: {#multiplication-directions-reset-button .interactive-button}
Reset
:::
:::
<script type="module" src="Scripts/Interactive/Quaternions/multiplication-directions.js"></script>

The logic can be applied to rotate any point around its origin.  

It is used in combination with simple vector arithmetic and [AngleAxis](AngleAxis.md) in [RotateAround](https://docs.unity3d.com/ScriptReference/Transform.RotateAround.html).  
Here is an annotated version of the logic:  

<<Code/Info/Quaternions/Multiplication 4.html>>  

### Usage - Rotating Orientations and Rotations
Quaternion multiplication is not commutative, `A * B != B * A`.  
Rotating `A` will influence `B` by rotating it as if its influence occurs as a parent of `B`.  
Rotating `B` will influence `A` by rotating it as if it's a local space modification on `A`'s orientation.  
To get an intuition about the influence of `A` or `B` on the resulting rotation, interact with the diagram below.

#### Interactive Diagram

::: {#multiplication .interactive-content}
::: {#multiplication-reset-button .interactive-button}
Reset
:::
:::
<script type="module" src="Scripts/Interactive/Quaternions/multiplication.js"></script>

```csharp
transform.rotation = A.rotation * B.rotation;
```