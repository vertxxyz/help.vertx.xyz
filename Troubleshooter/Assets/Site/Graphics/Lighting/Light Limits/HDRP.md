## Light limits: HDRP
### Description
HDRP's flexible light limits are not per-object, but are instead on-screen limits.  
HDRP is a [hybrid tile and cluster-based renderer](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/HDRP-Features.html#lighting-architecture), which means that lights are evaluated in a grid on-screen, the lighting limits may visually appear as a 2D grid due to the nature of this architecture.  

:::info
HDRP currently has a fixed[^1] 24 lights per-object limit.
:::

### Resolution

<<Graphics/Lightmapping.md>>

#### Dynamic lighting
If you wish to use **dynamic lights**: reducing their range, or substituting many specific lights for fewer more general ones, can help towards a functioning lighting setup.  
If the light limit manifests as tile-based (a 2D grid) then lastly, increasing the maximum value of the relevant [Lights setting](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/HDRP-Asset.html#lights) of the pipeline asset at the cost of performance can fix the issue.

[^1]: If working in a setting where performance isn't an issue, enabling the *Custom Frame Settings* option on the rendering Camera, overriding **and** disabling Deferred Tile in the Light Loop Debug setting will override the 24 lights per-object limit at a massive performance cost.