## [Quaternions](https://docs.unity3d.com/ScriptReference/Quaternion.html)
### What are they?
Nobody using them practically really needs to know.  
They're compact fancy four dimensional values that allow for simple *slerping* (spherically interpolating) between values. That is, to rotate to another orientation via the most direct rotation.  

### What aren't they?
**Quaternions are not Euler angles**.  
What this means:  
- They are **not** a 3-axis system, they should not be considered rotations about `x`, `y`, `z`.
- Their individual components are **not** to be used.  
  `x`, `y`, `z`, and `w` are for advanced use cases only.
- They cannot represent rotations of more than 180 degrees.

### What do they represent?
A Quaternion can represent either an **orientation** or a **rotation**.  
An orientation defines a rotational placement, an application of a rotation from the the default origin or "identity".  
A rotation is a rotational vector, something to manipulate another orientation or rotation.  
Sadly, just to confuse everyone **rotation** is a general term and can be used to also describe an orientation. `transform.rotation` is that Object's orientation.  

### The API
#### Rotation Construction
- [Quaternion.identity](Quaternions/Identity.md)
- [Quaternion.Euler](Quaternions/Euler.md)
- [Quaternion.AngleAxis](Quaternions/AngleAxis.md)
- [Quaternion.LookRotation](Quaternions/LookRotation.md)
- [Quaternion.FromToRotation](Quaternions/FromToRotation.md)

#### Rotation Modification and Application
- [Quaternion.Inverse](Quaternions/Inverse.md)
- [* (Multiplication)](Quaternions/Multiplication.md)