## [Quaternion.FromToRotation](https://docs.unity3d.com/ScriptReference/Quaternion.FromToRotation.html)
### Declaration
<<Code/Info/Quaternions/FromToRotation 1.html>>

### Description
A rotation from one direction to another. This will be the most direct rotation.  
When used as an orientation it is as if the ::`fromDirection`::{.quaternion-from} axis is rotated to point in ::`toDirection`::{.quaternion-to}, so it is used internally for `transform.right` and `transform.up`[^1]:  

<<Code/Info/Quaternions/FromToRotation 2.html>>  

### Interactive Diagram

::: {#from_to_rotation .interactive-content}
:::
<script type="module" src="Scripts/Interactive/Quaternions/fromToRotation.js"></script>  

<<Code/Info/Quaternions/FromToRotation 3.html>>  

There is an equivalence to [AngleAxis](AngleAxis.md) here, the two vectors' ::`cross product`::{.quaternion-axis} defines the axis, and the ::`angle`::{.quaternion-angle} is made between them.  
Similarly, the starting point around that axis is arbitrary, though defined by ::`fromDirection`::{.quaternion-from}, the resulting rotation angle is just a scalar around the axis.

---  

[^1]: `transform.forward` uses [LookRotation](LookRotation.md).