## TextMesh Pro: Incorrect material

TextMesh Pro typically uses signed distance fields to cheaply represent fonts at multiple distances and sizes. This requires specialised materials to function correctly. If a font was previously generated using a bitmap mode, or a material had its shader switched, this may cause the font to render incorrectly.

### Resolution
Switch the material used by the font asset to use one of the TextMesh Pro **Distance Field** shaders.

---

[My text is still blurry and clipped.](Padding%20To%20Size.md)