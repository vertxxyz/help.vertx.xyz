## Other considerations for 3D physics messages
### Correct Colliders
Ensure the dimension of the colliders and rigidbodies are 3D.  
Do not mix different types of colliders and messages.  
2D and 3D collision systems are separate, and do not interact with each other.
### Collision detection
If your objects are moving very quickly, you may need to set the rigidbody's **Collision Detection** mode to [**Continuous Dynamic**](https://docs.unity3d.com/Manual/ContinuousCollisionDetection.html). This will come with a performance overhead.
### The obvious
Check that your script is attached to one of the objects involved in the collision.