## Light limits: HDRP
HDRP's flexible light limits are not per-object, but are instead on-screen limits.  
HDRP is a [hybrid tile and cluster-based renderer](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/HDRP-Features.html#lighting-architecture), which means that lights are evaluated in a grid on-screen, the lighting limits may visually appear as a 2D grid due to the nature of this architecture.  

^^^
| Rendering Path | Light limit per object | Light limit per camera      |
|----------------|------------------------|-----------------------------|
| **Forward**    | unlimited              | 63 per 8x8 pixel tile.      |
| **Deferred**   | unlimited              | 63 per 16x16 pixel cluster. |
^^^ Find these limits documented on the [feature comparison page](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/Feature-Comparison.html).

### Resolution

<<Graphics/Lightmapping.md>>

#### Dynamic lighting
If you wish to use **dynamic lights**: reducing their range, or substituting many specific lights for fewer more general ones, can help towards a functioning lighting setup.  
If the light limit manifests as tile-based (a 2D grid) then lastly, increasing the maximum value of the relevant [Lights setting](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/HDRP-Asset.html#lights) of the pipeline asset at the cost of performance can fix the issue.

When working in a setting where performance isn't critical, enabling the *Custom Frame Settings* option on the rendering Camera, overriding **and** disabling Deferred Tile in the Light Loop Debug setting will override the limits at a large performance cost.
