### ScreenToWorldPoint - Spaces
#### Description
ScreenToWorldPoint will transform a position from Screen Space to World Space.  
This world-space position is relative to the Camera.  
If the position that results from a correct ScreenToWorldPoint call isn't resulting in a position you expect, then confirm that the target is actually intended to be in world space.

#### Resolution
[Draw a ray](../Debugging/Draw%20Functions.md) at the position returned by the ScreenToWorldPoint function to familiarise yourself with the position it returns.  
Compare that position with the object you are positioning. For example, if your object is parented under a Canvas that is **Screen Space - Overlay**, then that canvas is already in Screen Space, and just using the original mouse position values will do. If the canvas is **Screen Space - Camera**, then those object's positions will be world space, but their local positions will be in screen space.  

If you continue to have issues with this function, then simplify your logic and draw more rays to understand the issue.  
In some specific cases you may find that the camera position has been overridden for a small portion of the frame time. An issue like this will not appear in the scene view, but can affect camera functions for that period of the frame.