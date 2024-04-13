# Draw functions
Unity's [`Debug.DrawRay`](https://docs.unity3d.com/ScriptReference/Debug.DrawRay.html) and [`Debug.DrawLine`](https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html) are valuable tools for debugging 3D (and 2D) information.  
By drawing lines in the Scene and Game view you can validate assumptions about positions and directions used in code.

## Usage
Lines that aren't drawn continuously will only appear for a single frame, to counteract this a **duration** can be provided as the fourth parameter.  
Make sure that the variables used in your draw functions are the same as those used by the functionality you are debugging.

### DrawRay
Note that `DrawRay` takes a position and a **direction**.  
Scaling a *normalized* vector will produce a vector with that length. This can be done here to make the output more visible.  
<<Code/Drawing/Draw Functions 1.rtf>>

If you're passing two positions to this function the results will be unexpected. Use `DrawLine` instead.

### DrawLine

<<Code/Drawing/Draw Functions 2.rtf>>

## Visibility

To display gizmos in the Scene or Game view, toggles for each must be enabled.

![Scene view gizmo toggle](../Scene%20View/scene-view-gizmo-toggle.png)

---
[Return to visual debugging.](Visual%20Debugging.md)
