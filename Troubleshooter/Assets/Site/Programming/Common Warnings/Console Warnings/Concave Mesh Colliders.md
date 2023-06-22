## Concave mesh colliders and rigidbodies

```
PhysX does not support concave Mesh Colliders with dynamic Rigidbody GameObjects.
Either make the Mesh Collider convex, or make the Rigidbody kinematic. Scene hierarchy path "Foo", Mesh asset path "Bar" Mesh name "Baz"
```

### Description
You cannot use concave mesh colliders with dynamic rigidbodies.

### Resolution
**Either:**  
:::note
#### Use a convex mesh collider
If you don't need the concave (indented) portions of your collider to be accurate, check **convex** on your mesh collider.  
:::  
**Or:**  
:::note
#### Create a compound collider
Compound colliders are combinations of primitive colliders, collectively acting as a single rigidbody.  
To create a compound collider, create child objects of your colliding object, then add a Collider component to each child.

This easily allows you to position, rotate, and scale each collider independently of one another.

You can build your compound collider out of a number of primitive colliders and/or convex mesh colliders.  
:::  
**Or:**  
:::note
#### Use a kinematic rigidbody
If your body doesn't need to respond to collisions or move with forces, check **kinematic** on your rigidbody.  
:::