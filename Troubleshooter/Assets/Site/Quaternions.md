---
title: "Quaternions"
description: "Quaternions are not Euler angles (XYZ). Use their helper functions and don't be frightened."
---
# [Quaternions](https://docs.unity3d.com/ScriptReference/Quaternion.html)
## What are they?
Quaternions are compact four dimensional values that can be easily interpolated between values (to rotate to another orientation via the most direct rotation).

To use them practically you don't need to understand them beyond that simplified description.

## What aren't they?
**Quaternions are not [Euler angles](https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html)**.  
What this means:
- They are **not** a 3-axis system, they should not be considered rotations about `x`, `y`, `z`.
- Their individual components are **not** to be used.  
  `x`, `y`, `z`, and `w` are for advanced use cases only.
- They cannot represent rotations of more than 180 degrees.
- They don't suffer from [gimbal lock](https://www.youtube.com/watch?v=zc8b2Jo7mno).

## What do they represent?
Confusingly just as a `Vector3` can represent a position or a direction, a quaternion can represent either an **orientation** or a **rotation**.  
An orientation is a rotational placement, similar to position. `transform.rotation` is the global orientation of a transform.  
A rotation is a manipulation of another orientation or rotation, similar to a movement vector.

Sadly, rotation is a general term and is used to describe both orientations and rotations.

## The API
### Rotation construction
- [Quaternion.identity](Quaternions/Identity.md)
- [Quaternion.Euler](Quaternions/Euler.md)
- [Quaternion.AngleAxis](Quaternions/AngleAxis.md)
- [Quaternion.LookRotation](Quaternions/LookRotation.md)
- [Quaternion.FromToRotation](Quaternions/FromToRotation.md)

### Rotation modification and application
- [* (Multiplication)](Quaternions/Multiplication.md)
- [Quaternion.Inverse](Quaternions/Inverse.md)


## Issues
- [Reading individual xyz values from a Quaternion.](Quaternions/Members.md)
