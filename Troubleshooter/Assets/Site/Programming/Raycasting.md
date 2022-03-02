## Raycasting
### Colliders
Raycasting requires physics colliders to work.  
You cannot raycast against raw meshes using the `Raycast` functions!  

### Physics systems

2D colliders cannot be hit by [`Physics.Raycast`](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html), requiring [`Physics2D.Raycast`](https://docs.unity3d.com/ScriptReference/Physics2D.Raycast.html) instead.  
The 2D and 3D physics systems are entirely separate!

---  
- [I am using a Layer Mask.](Raycasting/Layer%20Masks.md)
- [I am **not** using Layer Mask.](Raycasting/Visual%20Debugging.md)