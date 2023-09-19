## Quaternion members

[Quaternions](https://docs.unity3d.com/Manual/class-Quaternion.html) are not [Euler angles](https://en.wikipedia.org/wiki/Euler_angles), they are **not** a 3-axis system and should not be considered rotations about `xyz`.

:::error  
Do not read the individual components from a `Quaternion`.  
`xyz`, and `w` are for advanced use cases only.  
:::

### Resolution

#### Read from Euler angles
Read from [`localEulerAngles`](https://docs.unity3d.com/ScriptReference/Transform-localEulerAngles.html) or [`eulerAngles`](https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html) instead of a `Quaternion` like [`transform.rotation`](https://docs.unity3d.com/ScriptReference/Transform-rotation.html). These are `Vector3` properties that represent the traditional XYZ rotations you are likely familiar with.

:::note
#### Note
Angles read in code may differ from those displayed in the Inspector, this is because they are derived from the internal representation, which is a `Quaternion`.  
If you can't work with this, consider either:
- Keeping track of your own set of angles which you update the transform with.
- Using another method to do what you want. Look into [vectors](https://docs.unity3d.com/Manual/VectorCookbook.html) and familiarise yourself with common operations like the [dot product](https://docs.unity3d.com/ScriptReference/Vector3.Dot.html).
- Using a compound transform. Multiple nested transforms where each one isolates an axis of rotation you are using.

Elaborating further: the inspector often displays user-authored rotations which have not undergone any transformation. You can set rotations beyond 180°, as what you see here is for authoring convenience. However, Quaternions don't over-rotate, and their representation is completely different though they both represent the same orientation.  
:::

### Documentation
#### Manual
- [Important classes: Quaternion](https://docs.unity3d.com/Manual/class-Quaternion.html)
- [Important classes: Vectors](https://docs.unity3d.com/Manual/class-Quaternion.html)

#### API Reference
- [`Quaternion`](https://docs.unity3d.com/ScriptReference/Quaternion.html)
- [`Vector3`](https://docs.unity3d.com/ScriptReference/Vector3.html)

---

See [Quaternion overview](../../Info/Quaternions.md) for more information.