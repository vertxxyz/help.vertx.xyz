## [ScreenToWorldPoint](https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html)

`ScreenToWorldPoint` takes a `Vector3` argument, where the first two values are screen coordinates, and the third is the distance from the camera.  
Providing distance is important when using a **perspective camera**, as `0` distance will return the world space camera position.

### Resolution
Provide a distance in the z coordinate of the screen position.

<<Code/Specific/ScreenToWorldPoint.rtf>>

:::info
If appropriate, switch your camera to Orthographic projection. This is the correct approach when making entirely 2D games.
:::  

#### Interactive Diagram

:::note{.center}
Drag the point to modify mouse position, move the slider to change the distance.
:::

:::hidden {#camera-img}
![Camera](camera.svg)
:::
<script type="module" src="/Scripts/Interactive/ScreenToWorldPoint/scene.js?v=1.0.0"></script>
<canvas id="screen_to_world_point" width="500" height="500"></canvas>
:::slider {#screen_to_world_point_slider}
:::

### Alternate methods
#### Using a Plane

Using a [`Plane`](https://docs.unity3d.com/ScriptReference/Plane.html) and [`ScreenPointToRay`](https://docs.unity3d.com/ScriptReference/Camera.ScreenPointToRay.html) avoids having to calculate an appropriate distance from the camera. When the result is on a fixed plane in space, this can be a suitable approach.  

<<Code/Specific/Plane Raycast.rtf>>  

#### Using Physics
Using a [`Raycast`](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) and [`ScreenPointToRay`](https://docs.unity3d.com/ScriptReference/Camera.ScreenPointToRay.html) avoids having to calculate an appropriate distance from the camera. When the result is meant to be on the surface of colliders, this is the correct approach.

```csharp
Ray ray = camera.ScreenPointToRay(Input.mousePosition);
if (Physics.Raycast(ray, out RaycastHit hit))
{
    // Use hit.point
}
```

---  

[The position returned is still incorrect.](ScreenToWorldPoint%20Spaces.md)