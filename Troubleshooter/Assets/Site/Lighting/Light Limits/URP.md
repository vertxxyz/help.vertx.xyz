# Light limits: URP
 
URP has per-object and per-camera light limits. 

^^^
+----------------+-----------------------------------------------------+--------------------------------------------+  
| Rendering Path | Light limit per object                              | Light limit per camera                     |  
+================+=====================================================+============================================+  
| **Forward**    + **4** for GLES2, **8** for all other graphics APIs. | On mobile platforms: **32**.<br/>
+----------------+-----------------------------------------------------+ On OpenGL ES 3.0 and earlier: **16**.<br/>
| **Forward+**   + unlimited                                           | On other platforms: **256**.
+----------------+-----------------------------------------------------+
| **Deferred**   + unlimited                                           |
+----------------+-----------------------------------------------------+--------------------------------------------+  
^^^ Find these limits documented on the [feature comparison page](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/universalrp-builtin-feature-comparison.html).

## Resolution
### Switching render path
[Forward+](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/rendering/forward-plus-rendering-path.html) is generally the most flexible render pipeline, with high light limits and complex shader support.  

<<Graphics/Lightmapping.md>>

### Dynamic lighting 
If you wish to use **dynamic lights**: reducing their range, substituting many specific lights for fewer more general ones, and lastly (at the cost of performance) increasing the [Per-Object Limit setting](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/universalrp-asset.html#lighting) in the pipeline asset can help towards a functioning lighting setup.
