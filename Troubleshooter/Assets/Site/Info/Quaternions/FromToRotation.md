## [Quaternion.FromToRotation](https://docs.unity3d.com/ScriptReference/Quaternion.FromToRotation.html)
### Declaration
<<Code/Info/Quaternions/FromToRotation 1.html>>

### Description
Creates a rotation from one direction to another. This will be the most direct rotation.  
When applied as an orientation, the ::`fromDirection`::{.quaternion-from} axis is rotated to point in ::`toDirection`::{.quaternion-to}.

### Interactive diagram

::: {#from_to_rotation .interactive-content}
:::
<script type="module" src="/Scripts/Interactive/Quaternions/fromToRotation.js"></script>  

<<Code/Info/Quaternions/FromToRotation 3.html>>  

There is an equivalence to [AngleAxis](AngleAxis.md) here, axis is the ::`cross product`::{.quaternion-axis}, and ::`angle`::{.quaternion-angle} is made between them.  
Similarly, the starting point around that axis is arbitrary, though defined by ::`fromDirection`::{.quaternion-from}, the angle is scalar.

### Usage

Setting `transform.right` and `transform.up` uses FromToRotation[^1].  
<<Code/Info/Quaternions/FromToRotation 2.html>>  

[^1]: `transform.forward` uses [LookRotation](LookRotation.md).