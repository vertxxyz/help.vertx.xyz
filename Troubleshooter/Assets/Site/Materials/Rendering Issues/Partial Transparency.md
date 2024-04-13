# Partial transparency

Different Material types have different ways of interpreting transparency.  
Some materials may improperly handle specularity with transparent surfaces, others may not fade entirely.

## Resolution
### Built-in Render Pipeline
The [Rendering Mode](https://docs.unity3d.com/Manual/StandardShaderMaterialParameterRenderingMode.html) setting on the Standard shader will control the way transparency is handled. Make sure you understand the difference between the **Transparent** and **Fade** options.

| Rendering Mode | Usage                                                                                                                                                                                                                                 |
|----------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Opaque         | For solid objects.                                                                                                                                                                                                                    |
| Cutout         | For solid to transparent objects with no transitional areas. Alpha is either 1 or 0.                                                                                                                                                  |
| Transparent    | For transparent objects where transparent (even totally transparent) areas can be affected by lighting and reflections. This is typical of materials like plastic or glass, but not for fading out portions of a material completely. |
| Fade           | For transparent objects that fade completely to nothing. Useful for fading objects out completely, including fading lighting and reflections.                                                                                         |

### Render pipelines
Disable **preserve specular lighting** if alpha is enabled.

---  
- [My object should appear partially cutout](Transparent%20To%20Cutout.md) - with solid and see-through parts.
- [My object should be transparent.](Rendering%20Mode.md)
