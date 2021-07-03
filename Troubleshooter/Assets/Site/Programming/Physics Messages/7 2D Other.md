### Other Considerations for 2D Physics Messages
#### Correct Colliders
Ensure the dimension of the colliders is 2D.  
Do not mix different types of colliders and messages.  
2D and 3D collision systems are separate, and do not interact with each other.
#### Collision Detection
If your objects are moving very quickly, you may need to set the rigidbody's **Collision Detection** mode to [**Continuous Dynamic**](https://docs.unity3d.com/Manual/ContinuousCollisionDetection.html). This will come with a performance overhead.
#### The Obvious
Check that your script is attached to one of the objects involved in the collision.