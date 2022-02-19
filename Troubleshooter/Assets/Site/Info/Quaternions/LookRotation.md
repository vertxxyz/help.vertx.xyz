## [Quaternion.LookRotation](https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html)
### Declaration
<<Code/Info/Quaternions/LookRotation 1.html>>

### Description
Creates a rotation with ::`forward`::{.quaternion-forward} and ::`upwards`::{.quaternion-up}.  
::`upwards`::{.quaternion-up} is used to guide the orientation, and ::`right`::{.quaternion-right} becomes the cross-product between it and ::`forward`::{.quaternion-forward}.  

Combined with [`Vector3.Cross`](https://docs.unity3d.com/ScriptReference/Vector3.Cross.html) this function is a staple for creating orientations.

### Interactive diagram

:::note{.center}
Drag the sphere near ::`forward`::{.quaternion-forward} or ::`upwards`::{.quaternion-up} to modify their direction.
:::

::: {#look_rotation .interactive-content}
:::
<script type="module" src="/Scripts/Interactive/Quaternions/lookRotation.js"></script>  

<<Code/Info/Quaternions/LookRotation 3.html>>

### In use

When setting [`transform.forward`](https://docs.unity3d.com/ScriptReference/Transform-forward.html), `LookRotation` is used[^1]:  
<<Code/Info/Quaternions/LookRotation 2.html>>  

A custom [`LookAt`](https://docs.unity3d.com/ScriptReference/Transform.LookAt.html) could choose to use it:

```csharp
public static Quaternion GetLookAtOrientation(Vector3 origin, Vector3 target)
    => GetLookAtOrientation(origin, target, Vector3.up);

public static Quaternion GetLookAtOrientation(Vector3 origin, Vector3 target, Vector3 up)
{
    Vector3 lookDirection = target - origin;
    return Quaternion.LookRotation(lookDirection, up);
}
```

---
Return to [Quaternions.](../Quaternions.md)  
Next, [Quaternion.FromToRotation.](FromToRotation.md)  

[^1]: [`transform.up`](https://docs.unity3d.com/ScriptReference/Transform-up.html) and [`transform.right`](https://docs.unity3d.com/ScriptReference/Transform-right.html) use [`FromToRotation`](FromToRotation.md).