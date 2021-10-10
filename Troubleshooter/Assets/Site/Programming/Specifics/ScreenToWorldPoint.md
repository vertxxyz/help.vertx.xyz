## ScreenToWorldPoint

### Issue:
ScreenToWorldPoint returns the camera position.  
ScreenToWorldPoint takes a `Vector3` parameter, where the first two values are screen coordinates. The third value is the distance from the camera.  
This value is important to provide when using a perspective camera, as a value of zero will just return the world space camera position.

### Resolution
Provide an appropriate distance in the Vector3 parameter.

:::hidden {#camera-img}
![Camera](camera.svg)
:::
<script src="Scripts/Interactive/ScreenToWorldPoint/scene.js"></script>
<canvas id="screen_to_world_point" width="500" height="500"></canvas>
:::slider {#screen_to_world_point_slider}
:::

<<Code/Specific/ScreenToWorldPoint.rtf>>

**Or**  

If appropriate, switch your camera to Orthographic projection. This is the correct approach when making entirely 2D games.

### Alternate Methods

Sometimes it is more suitable to use a [Plane](https://docs.unity3d.com/ScriptReference/Plane.html) to avoid calculating an appropriate distance from the camera. This is a suitable approach when the result is on a fixed plane in space.  

<<Code/Specific/Plane Raycast.rtf>>  

---
[The position returned is still incorrect.](ScreenToWorldPoint%20Spaces.md)