# Lightmapping
## Steps to baking lights

::::note
### 1. Create lightmap UVs for your models
:::note  
Enable **Generate Lightmap UVs** in the [model tab](https://docs.unity3d.com/Manual/FBXImporter-Model.html) of the Model Import Settings window.  
:::
**Or**  
:::note  
Populate the second UV channel in your modelling program of choice.  
:::  

---

You can check imported UV channels by selecting the imported Mesh sub-asset, and changing the preview window's Display Mode to UV Layout, and navigating to the UV channel. Your lightmap UVs should appear in Channel 1.
::::  

:::note  
### 2. Mark unmoving geometry as static
Enable [static flags](https://docs.unity3d.com/Manual/StaticObjects.html) on the GameObjects that you want to include in your bake.  
**Contribute GI** and **Reflection Probe Static** are the lighting relevant flags.  

Static objects cannot be moved in the scene at runtime.
If you want to light dynamic objects with baked lighting data see step **4**.
Certain small or complex objects may be unsuitable for lightmapping and also may benefit from step **4**.  
:::

:::note  
### 3. Mark your lights as baked
Select the lights that should contribute to your lightmaps and change the [**Mode**](https://docs.unity3d.com/Manual/LightModes.html) to [**Baked**](https://docs.unity3d.com/Manual/LightMode-Baked.html).  
You can use [**Mixed**](https://docs.unity3d.com/Manual/LightMode-Mixed.html) if you understand the scene's [Lighting Mode](https://docs.unity3d.com/Manual/lighting-mode.html) settings and how the effect this has on your bake.  
:::

:::note  
### 4. Use Light Probes for moving or complex objects
Moving objects cannot be included in baked lighting. Complex or small objects may also be unsuitable for lightmapping.
[Light Probes](https://docs.unity3d.com/Manual/LightProbes.html) exist to sample and blend baked lighting information for these dynamic objects.  

1. Create a [Light Probe Group](https://docs.unity3d.com/Manual/class-LightProbeGroup.html) and place its probes.
2. In the Renderer's settings ensure **Receive Global Illumination** is set to **Light Probes**.

::::info
If you use HDRP you can make use of [Adaptive Probe Volumes](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/probevolumes.html), which automate placement of light probes and provide per-pixel lighting.
::::  
:::

:::note  
### 5. Configure reflections
By default, the sky is sampled for reflections. This can often make dark areas appear bright and incorrectly lit.  
Configuring [Reflection Probes](https://docs.unity3d.com/Manual/ReflectionProbes.html) to provide approximate reflections for different areas of your scene will produce more accurate lighting.  
:::

:::note
### 6. Choose your Lightmapper
If you have a capable GPU, switch to the [Progressive GPU Lightmapper](https://docs.unity3d.com/Manual/GPUProgressiveLightmapper.html), as it will generally bake faster.  

1. Open the [Lighting window](https://docs.unity3d.com/Manual/lighting-window.html) (**Window | Rendering | Lighting**).
1. Under **Lightmapping settings | Lightmapper** select **Progressive GPU**

:::

:::note  
### 7. Generate lighting
1. Open the [Lighting window](https://docs.unity3d.com/Manual/lighting-window.html) (**Window | Rendering | Lighting**).
1. Select **Generate Lighting** at the bottom of the window.

:::

---

[Return to lighting.](Lighting.md)
