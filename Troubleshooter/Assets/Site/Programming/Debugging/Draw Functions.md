## Draw functions
### Description
Unity's [Debug.DrawRay](https://docs.unity3d.com/ScriptReference/Debug.DrawRay.html) and [Debug.DrawLine](https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html) are valuable tools for debugging 3D (and 2D) information.  
By drawing lines in the Scene and Game view you can validate assumptions about positions and directions used in code.
### Usage
Lines not drawn continuously will appear for a single frame, to counteract this a duration can be provided as the fourth parameter.  
Make sure that the variables used in your draw functions are the same as those used by the functionality you are debugging.  

:::info
If lines don't appear, ensure Gizmos are enabled for that view
:::

#### DrawRay
Note that `DrawRay` takes a position and a **direction**.  
Scaling a *normalized* vector will produce a vector with that length. This can be done here to make the output more visible.  
<<Code/Drawing/Draw Functions 1.rtf>>  

#### DrawLine

<<Code/Drawing/Draw Functions 2.rtf>>  

### Extra
I've provided a few custom debug drawing functions in a utility package I am developing that you can find [here](https://github.com/vertxxyz/Vertx.Debugging). This is a work in progress, but you can debug a many physics functions with custom gizmos, and draw labels at positions in the scene.