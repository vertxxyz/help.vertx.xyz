# Collider warnings (2D)
Check all colliders involved in the message have no warning messages on their components.

Warning messages like:

:::warning
The collider did not create any collision shapes as they failed verification.
:::

Can cause the collider to be completely non-existent, meaning it cannot be involved in any collisions.  
If there are warnings, do research to resolve them.  

### Common problems
- The collider's size is very small, no collider could be created.
- The collider is rotated such that it became too small in 2D perspective.

---

[I am still not getting a message.](9%202D%20Failed%20Compilation.md)
