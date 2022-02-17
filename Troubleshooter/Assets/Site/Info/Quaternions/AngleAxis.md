## [Quaternion.AngleAxis](https://docs.unity3d.com/ScriptReference/Quaternion.AngleAxis.html)
### Declaration
```csharp
public static Quaternion AngleAxis(float angle, Vector3 axis);
```

### Description
Creates a rotation which rotates ::`angle`::{.quaternion-angle} **degrees** around ::`axis`::{.quaternion-axis}.  
Euler angles are confusing to author when rotating off-axis. `AngleAxis` greatly simplifies this.  

<<Code/Info/Quaternions/AngleAxis 2.html>>

### Interactive diagram

::: {#angle_axis .interactive-content} 
:::
:::slider {#angle_axis_slider .color-angle} 
:::
<script type="module" src="/Scripts/Interactive/Quaternions/angleAxis.js"></script>  
<<Code/Info/Quaternions/AngleAxis 3.html>>  

Note that the rotation around axis does not have a starting position, it is purely scalar.

---
Return to [Quaternions](../Quaternions.md).