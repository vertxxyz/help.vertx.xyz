# UV mapping

Most commonly, the way materials map to surfaces is defined by a model's UV coordinates.  
These coordinates are authored in a modelling program. This is often called "UV unwrapping".  

## Resolution
### Authoring your own UVs
Author UVs for the model that appropriately tile the textures displayed on it.  

Uv mapping is a common exercise, research for your modelling program should be easy. See [Blender: UVs](https://docs.blender.org/manual/en/latest/modeling/meshes/uv/index.html).

### Triplanar mapping

There are various techniques for mapping textures onto surfaces dynamically. One is called triplanar mapping, where UVs are created by blending three projections (X, Y, Z directions) based on the normal of the point in question. Shader Graph has a [triplanar node](https://docs.unity3d.com/Packages/com.unity.shadergraph@latest/index.html?subfolder=/manual/Triplanar-Node.html) which returns these projected UVs.

UV mapping is cheaper as it's baked into the geometry, but it doesn't respond to dynamic situations like object scaling, and requires manual authoring.
