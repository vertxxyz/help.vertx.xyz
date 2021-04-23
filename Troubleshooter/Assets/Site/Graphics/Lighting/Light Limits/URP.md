### Light Limits - URP
#### Description
URP has per-object and per-camera light limits. Currently the limits are [8 per-object, and 256 per-camera](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/universalrp-builtin-feature-comparison.html).  
The per-object limit can be **reduced** in the pipeline asset, and defaults to **4**.

#### Resolution

<<Graphics/Lightmapping.md>>

**Dynamic Lighting**  
If you wish to use **dynamic lights**: reducing their range, substituting many specific lights for fewer more general ones, and lastly (at the cost of performance) increasing the [Per-Object Limit setting](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/universalrp-asset.html#lighting) in the pipeline asset can help towards a functioning lighting setup.