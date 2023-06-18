## Incorrect methods
### Description
There are various versions of raycast methods, use [the documentation](https://docs.unity3d.com/ScriptReference/) and check that you are using one that makes sense to your use-case.

### Resolution
If you are calling [`collider.Raycast(...`](https://docs.unity3d.com/ScriptReference/Collider.Raycast.html) from a collider, then you are using a method that can only hit that single collider.  
Instead, use the static method [`Physics.Raycast`](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) (or [`Physics2D.Raycast`](https://docs.unity3d.com/ScriptReference/Physics2D.Raycast.html)) that queries all the colliders in the scene.

---

[I am still having problems with my raycast.](Visual%20Debugging.md)