# [Quaternion.AngleAxis](https://docs.unity3d.com/ScriptReference/Quaternion.AngleAxis.html)
## Declaration
```csharp
public static Quaternion AngleAxis(float angle, Vector3 axis);
```

## Description
Creates a rotation which rotates ::`angle`::{.quaternion-angle} **degrees** around ::`axis`::{.quaternion-axis}.  
Euler angles are confusing to author when rotating off-axis. `AngleAxis` greatly simplifies this.  

<<Code/Info/Quaternions/AngleAxis 2.html>>

## Interactive diagram

:::note{.center}
Drag the sphere to modify ::`axis`::{.quaternion-axis}, move the slider to change ::`angle`::{.quaternion-angle}.
:::

::: {#angle_axis .interactive-content} 
:::
:::slider {#angle_axis_slider .color-angle} 
:::
<script type="module" src="/Scripts/Interactive/Quaternions/angleAxis.js?v=1.0.0"></script>  
<<Code/Info/Quaternions/AngleAxis 3.html>>  

:::note
Note that the rotation angle is scalar, it does not start at the visualised position.
:::  

---
Return to [Quaternions.](../Quaternions.md)  
Next, [Quaternion.LookRotation.](LookRotation.md)
