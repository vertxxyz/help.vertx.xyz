## Normal map format in Unity

Unity uses Y+ normal maps, known as OpenGL format.

### Example
If your texture is not of this color, you may be using the incorrect format.  
^^^
![Normal map example](https://docs.unity3d.com/uploads/Main/BumpMapTexturePreview.png)
^^^ This normal map has embossed surfaces.

Normal maps also need to be imported in a compressed format for Unity to sample them correctly.

### Resolution

1. Ensure your normal map is in the Y+ (OpenGL) format, programs that generate normal maps often have export options.  
   If you suspect the format to be incorrect and cant re-export, invert the green channel.
1. Set the [texture importer](https://docs.unity3d.com/Manual/texture-type-normal-map.html)'s **texture type** to **normal map**.  
   If you are using black and white texture, make sure **create from grayscale** is checked.

---

See [normal map (bump mapping)](https://docs.unity3d.com/Manual/StandardShaderMaterialParameterNormalMap.html) for more information.