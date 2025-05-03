# Normals and lighting
If a surface isn't lit correctly—a point light doesn't light a circular area for example—then you likely have an issue with **normals**.

Normals are vectors describing the direction a surface faces. Lighting won’t make sense when normals are configured incorrectly.

## Resolution
Different sources of normals can be incorrect:

:::note
### Vertex normals
Check that your model normals are correct.  
1. In the [model tab](https://docs.unity3d.com/Manual/FBXImporter-Model.html) of the Model Import Settings window set **Normals** to **Calculate**.
1. If your model is now lit correctly, you should set/recalculate normals in your modelling program before importing.

Validate your setup by comparing against a built-in model and material.  
:::

:::note
### Normal maps
If you're using a normal map, check that it's [imported correctly](https://docs.unity3d.com/Manual/StandardShaderMaterialParameterNormalMapImport.html).  
1. In the texture importer, set **Texture Type** to **Normal Map**.
1. Unity uses Y+ normal maps, sometimes known as OpenGL format. Check that your normal map is in the correct format.

Validate your setup by comparing against a material with no normal map applied.  
:::

:::note
### Shaders
If you're using a custom shader, you may have transformed the normals incorrectly.  
1. Normals generated in the vertex stage should be output in object space.
1. Normals generated in the fragment stage should be output in tangent space.

Validate your setup by comparing against default materials.  
:::
