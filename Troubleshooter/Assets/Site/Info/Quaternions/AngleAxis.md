## [Quaternion.AngleAxis](https://docs.unity3d.com/ScriptReference/Quaternion.AngleAxis.html)
### Declaration
```csharp
public static Quaternion AngleAxis(float angle, Vector3 axis);
```

### Description
To use `AngleAxis` provide ::`angle`::{.quaternion-angle} **degrees** to rotate a direction ::`axis`::{.quaternion-axis} as if it punctures an object.  

<<Code/Info/Quaternions/AngleAxis 2.html>>

This function is fantastic for constructing rotations around arbitrary axes. With Euler angles once rotations are multi-axis the results become confusing.

### Interactive Diagram

::: {#angle_axis .interactive-content} 
:::
:::slider {#angle_axis_slider .color-angle} 
:::
<script type="module" src="Scripts/Interactive/Quaternions/angleAxis.js"></script>  
<<Code/Info/Quaternions/AngleAxis 3.html>>  

Note that the pictured perpendicular "start" of the rotation is positioned arbitrarily.  
The rotation around the vector does not have a starting position, it is purely a scalar value.

---  
