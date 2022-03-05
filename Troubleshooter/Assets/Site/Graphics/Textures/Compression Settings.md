## Compression settings
### Description
To reduce file size textures are imported with a degree of compression by default.  
In some cases, like pixel art, or specific UI textures, compression isn't desired, and should be disabled.

Different platforms support different forms of compression, see [Supported texture compression formats, by platform](https://docs.unity3d.com/Manual/class-TextureImporterOverride.html) for format compatibility and effects.

### Resolution
In the [Platform-specific Overrides](https://docs.unity3d.com/Manual/class-TextureImporter.html#platform) section of the Texture Importer, reduce, or disable the compression quality of the texture for the valid platforms.  
In some cases changing the format may also be required, or may help reduce the amount of artifacts for a similar amount of compression.  

---  

[Colours still appear incorrect.](Game%20View%20Zoom.md)