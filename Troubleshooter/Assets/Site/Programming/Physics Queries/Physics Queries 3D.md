## Physics queries (3D): General
### Use the correct class
You must be using functions from the [`Physics`](https://docs.unity3d.com/ScriptReference/Physics.html) class. [`Physics2D`](https://docs.unity3d.com/ScriptReference/Physics2D.html) calls will not interact with 3D physics.

If you are calling a query like [`collider.Raycast(...`](https://docs.unity3d.com/ScriptReference/Collider.Raycast.html) from a collider, then you are using a method that can only hit that single collider.  
Make sure you understand this, or instead, use the static method [`Physics.Raycast`](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) that queries all the colliders in the scene.

### Issues with colliders
- Raycasting requires [colliders](https://docs.unity3d.com/Manual/CollidersOverview.html) to work. 2D colliders will not by hit by `Physics` queries.  
You cannot query against raw meshes using `Physics` functions.

- If you are using a Mesh Collider, be aware that changing the MeshFilter's mesh will not update the Mesh Collider.

---

[I am still having problems with my query.](Visual%20Debugging.md)