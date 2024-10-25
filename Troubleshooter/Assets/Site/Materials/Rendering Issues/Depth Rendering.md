# Depth rendering
Forcing a material to define its depth in space, at the cost of appearing solid to transparent objects that try to render behind it afterwards.  

If you are using custom shaders, add [ZWrite On](https://docs.unity3d.com/Manual/SL-ZWrite.html).  
URP and HDRP both have a [Render Objects](https://docs.unity3d.com/Manual/urp/renderer-features/renderer-feature-render-objects.html) feature that can be used if your version of Shader Graph does not have **Depth Write** exposed.  
