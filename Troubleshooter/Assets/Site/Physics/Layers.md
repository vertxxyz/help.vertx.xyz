### Layer Issues

Unity's layer-based collision can exclude colliders and triggers from interacting with each other at all.  
Check that the toggles in the project settings match with the two colliders in question.  

For **3D**:  
See **Edit | Project Settings | Physics**, and the **Layer Collision Matrix** at the bottom.  
For **2D**:  
See **Edit | Project Settings | Physics 2D**, and the **Layer Collision Matrix** at the bottom.  

![Layer Collision Matrix](../Programming/Physics%20Messages/collision-layer-matrix.png)

See [Layer-based collision detection](https://docs.unity3d.com/Manual/LayerBasedCollision.html) for more information.  

Make sure the layers of your two objects are interacting in this matrix.