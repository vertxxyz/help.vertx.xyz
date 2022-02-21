## Raycasting
### Basics
Raycasting requires physics colliders to work. You cannot raycast against raw meshes using the `Raycast` functions.  

2D colliders cannot be hit by the `Physics.Raycast` functions, and require `Physics2D.Raycast` instead. 2D and 3D physics are entirely separate.

---  
- [I am using a Layer Mask.](Raycasting/Raycasting%20Layer%20Masks.md)
- [I am **not** using Layer Mask.](Raycasting/Visual%20Debugging.md)