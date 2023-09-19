## Models rendering in black

### Materials
Check that your **material** is not tinted black.  
The material is the problem if it renders incorrectly when applied to a default primitive.

### Normals
Check that your model **normals** are correct.  
1. In the [model tab](https://docs.unity3d.com/Manual/FBXImporter-Model.html) of the Model Import Settings window set **Normals** to **Calculate**.  
1. If your model is no longer black, you should set/recalculate normals in your modelling program before importing.

### UVs
If you are using a texture, check that your model's **UVs** are correct.  
1. Download a UV test texture and use it in an unlit material.
1. Check that the texture appears reasonably on your model, if the texture is unexpectedly stretched or a single color you may need to [UV map your model](UV%20Mapping.md).

### Shaders
If you are using a non-standard shader, and are seeing a harsh-edged deep black on the screen your material may be producing values that are not a number (NaN).  
Sometimes NaN values are not an issue until propagated through other effects like post processing, and can be object-specific. This is commonly a division by 0.

There are other issues that can cause black output, but there are infinite shaders, so you will have to do your own investigations. Isolate the shader's output one by one until you find the cause.

### Lighting
Check that your scene is lit correctly. Recreate the issue in an clean scene with a directional light and compare lighting against default primitives.