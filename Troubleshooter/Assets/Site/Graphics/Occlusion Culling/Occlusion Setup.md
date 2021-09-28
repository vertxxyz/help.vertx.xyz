## Occlusion Setup
### Description
Occlusion culling is made up of both Occluders and Occludees.  
Occluders block visibility, and Occludees are visible based on them.

### Resolution
Selectively mark occluders as Occluder Static in the **Object** tab of the **Occlusion Window**.  
You can additionally mark some occludees as Occludee Static in the same window, this is for objects that do not move.    
You can filter the Scene Hierarchy to contain only Renderers which sometimes helps with this process.  
For dynamic, moving occludees you can mark a Renderer as having [Dynamic Occlusion](https://docs.unity3d.com/Manual/occlusion-culling-dynamic-gameobjects.html) in its additional settings tab, this is activated by default.

Then attempt to bake the Scene.