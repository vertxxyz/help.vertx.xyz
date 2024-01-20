## Quaternion members

[Quaternions](https://docs.unity3d.com/Manual/class-Quaternion.html) are not [Euler angles](https://en.wikipedia.org/wiki/Euler_angles), they are **not** a 3-axis system and should not be considered rotations about `xyz`.

:::error  
Do not read the individual components from a `Quaternion`.  
`xyz`, and `w` are for advanced use cases only.  
:::

### Resolution

#### Read from Euler angles
Read from [`localEulerAngles`](https://docs.unity3d.com/ScriptReference/Transform-localEulerAngles.html) or [`eulerAngles`](https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html) instead of a `Quaternion` like [`transform.rotation`](https://docs.unity3d.com/ScriptReference/Transform-rotation.html).  
These are `Vector3` properties that represent the traditional XYZ rotations you are likely familiar with.

```diff
-transform.rotation.x
+transform.eulerAngles.x
-transform.localRotation.x
+transform.localEulerAngles.x
```

:::note
#### Note
Angles read in code may differ from those displayed in the Inspector because they are derived from the internal representation, which is a `Quaternion`.  
If you can't work with this, consider either:
- Keeping track of your own set of angles which you use to update the transform.
- Using another method to do what you want. Look into [vectors](https://docs.unity3d.com/Manual/VectorCookbook.html) and familiarise yourself with common operations like the [dot product](https://docs.unity3d.com/ScriptReference/Vector3.Dot.html).
- Using a compound transform; multiple nested transforms, where each one isolates an axis of rotation you are using.

The inspector often displays user-authored rotations, where you can set rotations beyond 180° for authoring convenience.
However, Quaternions don't over-rotate, and when storing Euler angles as a Quaternion that information is lost. Although, the same orientation is represented in the end.  
:::

### Documentation
#### Manual
- [Important classes: Quaternion](https://docs.unity3d.com/Manual/class-Quaternion.html)
- [Important classes: Vectors](https://docs.unity3d.com/Manual/class-Quaternion.html)

#### API Reference
- [`Quaternion`](https://docs.unity3d.com/ScriptReference/Quaternion.html)
- [`Vector3`](https://docs.unity3d.com/ScriptReference/Vector3.html)

---

See [Quaternion overview](../Quaternions.md) for more information.
