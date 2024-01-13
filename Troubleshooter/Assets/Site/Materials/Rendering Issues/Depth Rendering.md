## Depth rendering
Forcing a material to define its depth in space, at the cost of appearing solid to transparent objects that try to render behind it afterwards.  

If you are using custom shaders, add [ZWrite On](https://docs.unity3d.com/2017.3/Documentation/Manual/SL-CullAndDepth.html).  
URP and HDRP both have a [Render Objects](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/urp-renderer-feature.html#render-objects-renderer-featurea-namerender-objects-renderer-featurea) feature that can be used if your version of Shader Graph does not have **Depth Write** exposed.  