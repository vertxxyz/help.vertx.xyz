# Continuous collision (3D)
If your objects are moving very quickly, you may need to set the rigidbody's **Collision Detection** mode to [**Continuous Dynamic**](https://docs.unity3d.com/Manual/ContinuousCollisionDetection.html).  

Continuous collision will attempt to sweep the collider, checking for collisions that occur between fixed update increments.  
This will come with a performance overhead.

---

[I am still not getting a message.](8%203D%20Collider%20Warnings.md)
