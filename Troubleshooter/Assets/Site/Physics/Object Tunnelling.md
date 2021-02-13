### Object Tunnelling
#### Issue
When rigidbody objects are moving very quickly, they can tunnel through surfaces because checks to resolve their intersections are made without consideration as to the path they have taken.

#### Resolution
Setting a rigidbody's **Collision Detection** mode to [**Continuous**](https://docs.unity3d.com/Manual/ContinuousCollisionDetection.html), or **Continuous Dynamic**. This will come with a performance overhead.  
This performs a sweep to determine whether the path the body has taken is interrupted.

![Rigidbody Collision Detection](http://help.vertx.xyz/Images/rigidbody-collision-detection.png)

---

[My rigidbody is still tunnelling through walls](Layers.md)