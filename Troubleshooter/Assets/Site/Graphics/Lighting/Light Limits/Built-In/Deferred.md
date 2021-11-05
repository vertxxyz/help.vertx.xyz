## Light limits: Built-in - deferred
With the [Deferred Shading rendering path](https://docs.unity3d.com/Manual/RenderTech-DeferredShading.html) there is no limit to the number of lights that can affect a GameObject.  
Though, semi-transparent objects will fall back to [Forward Rendering](Forward.md). If the objects are using shaders that support semi-transparency you will be restricted by those limits.

Otherwise, it is likely that the issue lies elsewhere. If none of the general lighting troubleshooting steps solve your issue, please reach out to me with the correct solution when found!