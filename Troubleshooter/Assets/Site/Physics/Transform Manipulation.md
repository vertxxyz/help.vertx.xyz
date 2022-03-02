## Transform manipulation
If you are moving dynamic rigidbodies via their transforms they are likely **not** interacting with the physics engine, and depending on the implementation may *only* resolve penetrations.

Using `transform.position` to move objects bypasses the physics simulation step.  
By instead using [`AddForce`](https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html), or [MovePosition](https://docs.unity3d.com/ScriptReference/Rigidbody.MovePosition.html), colliders will produce collisions and triggers.  
These methods should generally be called from [`FixedUpdate`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html).  

---  

[I am still not getting a message.](Layers.md)