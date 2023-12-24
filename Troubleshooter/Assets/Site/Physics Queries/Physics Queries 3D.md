## Physics queries (3D): General
### Use the correct class
You must be using functions from the [`Physics`](https://docs.unity3d.com/ScriptReference/Physics.html) class. [`Physics2D`](https://docs.unity3d.com/ScriptReference/Physics2D.html) calls will not interact with 3D physics.

If you are calling a query like [`collider.Raycast(...`](https://docs.unity3d.com/ScriptReference/Collider.Raycast.html) from a collider, then you are using a method that can only hit that single collider.  
Make sure you understand this, or instead, use the static method [`Physics.Raycast`](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) that queries all the colliders in the scene.

### Issues with colliders
- Raycasting requires [colliders](https://docs.unity3d.com/Manual/CollidersOverview.html) to work. 2D colliders will not by hit by `Physics` queries.  
You cannot query against raw meshes using `Physics` functions.
- If you are using a Mesh Collider, be aware that changing the MeshFilter's mesh will not update the Mesh Collider.
- Mesh Colliders are single-sided. Check that you are casting against the front face.
- Mesh Collider Meshes must be marked as read/write under certain circumstances (see [limitations](https://docs.unity3d.com/Manual/class-MeshCollider.html)), this may only affect builds.

### Issues with setup
Check the documentation for the function use are using. Many have notes on edge cases where behavior may be undefined or default.
#### Examples
- Raycasts will not detect colliders they originate inside.
- Shape **casts** will not detect colliders that overlap their starting shape. Consider using **overlaps** to detect them instead.
- Don't use **casts** of `0` `maxDistance`, or `zero` `direction`, consider using **overlaps** instead.
- Shape casts with multiple results will detect colliders they overlap at the start, but will return default values.  
  [`RaycastHit`](https://docs.unity3d.com/ScriptReference/RaycastHit.html) `distance` is zero, `point` is zero, and `normal` is the inverse of the ray direction.
- Passing in zero size/radius often produces undefined output.

---

[I am still having problems with my query.](NonAlloc%203D.md)
