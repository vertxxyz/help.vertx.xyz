# Clipping Planes

The near and far clipping planes describe two bounds of the volume rendered by a camera.  
The Scene view camera automatically adjusts its bounds when focusing objects, and can be adjusted manually.

## Resolution
Select an object and press <kbd>F</kbd> to focus the camera on the object.  
Focusing an object should reasonably adjust the clipping planes in relation to its scale.

To gain manual control of the clipping planes, disable **Dynamic Clipping** in the Scene view control bar's camera dropdown and adjust the clipping plane values.
Generally, dynamic clipping should be enabled unless specific requirements are needed.

---
[My scene view still has clipping problems.](Scene%20View%20Gizmo.md)
