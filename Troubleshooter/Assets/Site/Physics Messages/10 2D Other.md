## Other considerations for 2D physics messages

### Correct Colliders
Ensure the dimension of the colliders and rigidbodies are 2D.  
Don't mix different types of colliders and messages.  
2D and 3D collision systems are separate, and do not interact with each other.

### The obvious
1. Your script must be attached to one of the objects involved in the collision.
1. The colliders on your objects must be enabled.
1. Rigidbodies involved in the collision must have **Simulated** enabled.

---
If you resolved your issue and the fix was not listed in the [troubleshooting steps](../Physics%20Messages.md), please <<report-issue.html>>.