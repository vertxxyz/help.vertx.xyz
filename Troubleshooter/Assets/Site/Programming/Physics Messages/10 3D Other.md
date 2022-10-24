## Other considerations for 3D physics messages

### Correct Colliders
Ensure the dimension of the colliders and rigidbodies are 3D.  
Do not mix different types of colliders and messages.  
2D and 3D collision systems are separate, and do not interact with each other.

### The obvious
Check that your script is attached to one of the objects involved in the collision.