# Transform manipulation (3D)
If you are moving dynamic (non-kinematic) rigidbodies via their transforms they are likely **not** interacting with the physics engine, and will not properly send messages.

Using `transform.position` to move objects bypasses the physics simulation step.  
By instead using [`AddForce`](https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html), or [`MovePosition`](https://docs.unity3d.com/ScriptReference/Rigidbody.MovePosition.html), colliders will produce collisions and triggers.  
These methods should generally be called from [`FixedUpdate`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html).

## Creating rigidbody character controllers
If you are confused as to how to write a character controller that uses a `Rigidbody`, watch a tutorial or purchase a pre-made controller. Unity has Starter Assets with [third person](https://assetstore.unity.com/packages/essentials/starter-assets-third-person-character-controller-urp-196526) and [first person](https://assetstore.unity.com/packages/essentials/starter-assets-first-person-character-controller-urp-196525) character controllers, these assets are set up for the [Universal Render Pipeline](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/). For a more robust controller Unity also owns a [Kinematic Character Controller](https://assetstore.unity.com/packages/tools/physics/kinematic-character-controller-99131) which contains a demo controller, but requires experience to compose into your own setup.

### Physics-based controllers
Physics-based controllers will use forces to move a body through the scene, meaning they are fairly easy to create but can respond to forces in ways that may not be desirable. This can include incorrect slope handling, poor control on bumpy surfaces, unexpected responses to external forces, and so on. 
### Kinematic controllers
Kinematic controllers use physics queries to check for movement before directly moving to a location. Extra work is required to build a complete controller that robustly handles complex scenarios, and bypassing physics can make the character unresponsive to external forces.  
However, because each aspect of their interactions has been authored, this makes for a robust and intentional experience.

---  

[I am still not getting a message.](7%203D%20Continuous%20Detection.md)
