## Layer issues

Unity's layer-based collision can exclude colliders and triggers from interacting with each other at all.  
Check that the toggles matching the colliders layers are enabled in the project settings.  

For **3D**:  
See **Edit | Project Settings | Physics**, and the **Layer Collision Matrix** at the bottom.  
For **2D**:  
See **Edit | Project Settings | Physics 2D**, and the **Layer Collision Matrix** at the bottom.  

![Layer Collision Matrix](../Programming/Physics%20Messages/collision-layer-matrix.png)

See [Layer-based collision detection](https://docs.unity3d.com/Manual/LayerBasedCollision.html) for more information.  

Make sure the layers of your two objects are interacting in this matrix.