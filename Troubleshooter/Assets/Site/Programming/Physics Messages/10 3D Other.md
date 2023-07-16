## Other considerations for 3D physics messages

### Correct Colliders
Ensure the dimension of the colliders and rigidbodies are 3D.  
Don't mix different types of colliders and messages.  
2D and 3D collision systems are separate, and do not interact with each other.

### The obvious
1. Your script must be attached to one of the objects involved in the collision.
1. The colliders on your objects must be enabled.
1. Mesh colliders must have meshes assigned.
1. Do not disable [`detectCollisions`](https://docs.unity3d.com/ScriptReference/Rigidbody-detectCollisions.html) for involved rigidbodies.
1. Do not disable [`Physics.invokeCollisionCallbacks`](https://docs.unity3d.com/2023.2/Documentation/ScriptReference/Physics-invokeCollisionCallbacks.html) (unless solely handling events with [`Physics.ContactEvent`](https://docs.unity3d.com/2023.2/Documentation/ScriptReference/Physics.ContactEvent.html)).

---
If you resolved your issue and the fix was not listed in the [troubleshooting steps](../Physics%20Messages.md), please <<report-issue.html>>.