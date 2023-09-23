## ScreenToWorldPoint: Spaces

ScreenToWorldPoint will transform a position from **Screen Space** to **World Space**.
This world space position is generated with the Camera, but is relative to the origin.

If the position returned by a correct `ScreenToWorldPoint` call isn't expected, confirm that the target is actually intended to be in world space.

### Resolution
[Draw a ray](../../Debugging/Draw%20Functions.md) at the position returned by `ScreenToWorldPoint` to familiarise yourself with the position it returns.

Compare that position with the object you are positioning. For example, if your object is parented under a [Canvas](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/UICanvas.html) that is **Screen Space - Overlay**, then that canvas may already be in Screen Space, and using the original mouse position values may do (a [Canvas Scaler](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-CanvasScaler.html) may scale this space).
If the canvas is **Screen Space - Camera**, then those object's positions will be world space, but their local positions will be in screen space.

If you continue to have issues with this function, simplify your logic and draw more rays to understand the issue.
In some specific cases you may find that the camera position has been overridden for a small portion of the frame time. An issue like this will not appear in the Scene view, but can affect camera functions for that period of the frame. Drawing rays will indicate whether this is the case.

---
See [vectors: positions and directions](../Vectors/Positions%20And%20Directions.md) for more info on spaces and vectors.
