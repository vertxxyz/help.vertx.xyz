## Layer Collision Matrix

### Global
Unity's layer-based collision can exclude colliders and triggers from interacting with each other at all.  
Check that the toggles matching the colliders layers are enabled in the project settings.

See **Edit | Project Settings | Physics**, and the **Layer Collision Matrix** at the bottom.  

![Layer Collision Matrix](collision-layer-matrix.png)  

See [Layer-based collision detection](https://docs.unity3d.com/Manual/LayerBasedCollision.html) for more information.  

### Local
Individual rigidbody components have **layer overrides** that are merged with the global settings.  
By default, **include layers** and **exclude layers** should both be set to **nothing**.  
If you are using these settings, be aware of their impact.

---
[I am still not getting a message.](6%203D%20Transform.md)