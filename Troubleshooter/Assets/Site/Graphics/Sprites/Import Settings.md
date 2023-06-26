## Sprites: Import settings

Textures are not imported as [sprites](https://docs.unity3d.com/Manual/Sprites.html) by default.  
Multiple sprites can be generated from a single texture atlas, and sprites allow for slicing and collision settings that normal textures do not have.

### Resolution
In the [Texture Importer](https://docs.unity3d.com/Manual/class-TextureImporter.html#texturetype) change the **texture type** to [**Sprite (2D and UI)**](https://docs.unity3d.com/Manual/TextureTypes.html#sprite-2d-and-ui), this will tell Unity to generate a sprite.  
Individual sprites are now created as sub-assets (expand the foldout now present on the asset).

---

If you're importing a number of textures with similar settings you can use a [Preset](https://docs.unity3d.com/Manual/Presets.html) and apply it to a multiple assets, to folders, or to the entire project.