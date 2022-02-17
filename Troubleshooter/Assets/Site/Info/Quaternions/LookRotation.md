## [Quaternion.LookRotation](https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html)
### Declaration
<<Code/Info/Quaternions/LookRotation 1.html>>

### Description
Creates a rotation with ::`forward`::{.quaternion-forward} and ::`upwards`::{.quaternion-up}.  
::`upwards`::{.quaternion-up} is used to guide the orientation, and ::`right`::{.quaternion-right} is the cross-product between it and ::`forward`::{.quaternion-forward}.  

Combined with [Vector3.Cross](https://docs.unity3d.com/ScriptReference/Vector3.Cross.html) this function is a staple for creating orientations aligned to surfaces.

### Interactive diagram

::: {#look_rotation .interactive-content}
:::
<script type="module" src="/Scripts/Interactive/Quaternions/lookRotation.js"></script>  

<<Code/Info/Quaternions/LookRotation 3.html>>

### Usage

Setting `transform.forward` uses LookRotation[^1]:  
<<Code/Info/Quaternions/LookRotation 2.html>>  

A custom [LookAt](https://docs.unity3d.com/ScriptReference/Transform.LookAt.html) could also choose to use this function:

```csharp
public static Quaternion GetLookAtOrientation(Vector3 origin, Vector3 target)
    => GetLookAtDirection(origin, target, Vector3.up);

public static Quaternion GetLookAtOrientation(Vector3 origin, Vector3 target, Vector3 upwards)
{
    Vector3 lookDirection = target.position - origin.position;
    return Quaternion.LookRotation(lookDirection, upwards);
}
```

---
Return to [Quaternions](../Quaternions.md).

[^1]: `transform.up` and `transform.right` use [FromToRotation](FromToRotation.md).