# Light limits: Built-in - forward
Forward rendering is lighting strategy that manually handles the rendering of lights individually on a per-object basis.  
These lights are prioritised based on their Importance, brightness, and distance.  
Their quality can also be affected by these metrics, as their accuracy is reduced to different levels of detail as their importance diminishes.

## Resolution

<<Graphics/Lightmapping.md>>

### Switching to deferred
If your project intends to support **many dynamic lights** often it can be beneficial to switch the to the [deferred shading render path](Deferred.md).  
This can come with drawbacks: deferred does not support hardware antialiasing (MSAA), orthographic projection, per-object shadow disabling, and culling masks are limited.  
See [Rendering Path comparison](https://docs.unity3d.com/Manual/RenderingPaths.html) for more information.

### Forward rendering
If you wish to use **forward rendering and dynamic lights**: reducing their range, substituting many specific lights for fewer more general ones, and lastly (at the cost of performance) increasing the [Pixel Light Count setting](https://docs.unity3d.com/Manual/class-QualitySettings.html) in the Quality settings can help towards a functioning lighting setup.

---
See [Introduction to Lighting and Rendering](https://learn.unity.com/tutorial/introduction-to-lighting-and-rendering) for a step-by-step process on lighting considerations.
