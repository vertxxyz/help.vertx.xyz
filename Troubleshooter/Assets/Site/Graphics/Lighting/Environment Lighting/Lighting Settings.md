## Environment Lighting - Settings
### Description
Realtime lighting does not produce indirect lighting by itself.  
Indirect lighting is the light that bounces off surfaces and contributes to lighting behind objects.  
Environment lighting is the contribution of additional lighting from the Scene from sources like the Skybox or reflections.  

### Resolution
Consider your options for adding the appearance of environment lighting. Many of these options are in the [Lighting window](https://docs.unity3d.com/Manual/lighting-window.html).  
1. Add an environment lighting colour or gradient to the Scene. See the [Environment tab](https://docs.unity3d.com/Manual/lighting-window.html#EnvironmentSection).  
This option will globally add to the lighting, but is the cheapest and fastest option.
2. Bake your lighting. See the [Lightmapping Settings](https://docs.unity3d.com/Manual/Lightmapping.html).  
Lightmapping is the process of baking light contribution and bounce to textures. This information is later sampled to produce an accurate, but static lighting setup. Light mapping takes time in the editor, and can contribute largely to build size, but is cheap to evaluate at runtime.  
Dynamic objects can have lightmaps contribute to their lighting through the use of [Light Probes](https://docs.unity3d.com/Manual/LightProbes.html)
3. Add custom environment lighting to your material setup. Custom shaders can add any level and complexity of custom lighting, this is entirely project specific.  
In some cases this can be as simple as choosing an unlit material, where no faces are shaded based on lighting.  
5. If supported, add Realtime Global Illumination (RTGI). [Enlighten](https://docs.unity3d.com/Manual/realtime-gi-using-enlighten.html) is the built-in solution. It has been deprecated, and in certain scenarios has been returned to the engine as a stopgap between Unity developing their own modern solution. Enlighten requires baking time in the editor, and has significant runtime performance impact.