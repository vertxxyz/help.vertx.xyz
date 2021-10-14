## Occlusion Setup
### Description
Occlusion culling is made up of both Occluders and Occludees.  
Occluders block visibility, and Occludees are visible based on them.

### Resolution
Selectively mark occluders as Occluder Static in the **Object** tab of the **Occlusion window**.  
Occludees can be marked as Occludee Static in the same window, this is for objects that do not move.    
The Scene Hierarchy can be filtered to contain only Renderers which sometimes helps with this process.  
The Renderers of Dynamic, moving occludees can be marked as having [Dynamic Occlusion](https://docs.unity3d.com/Manual/occlusion-culling-dynamic-gameobjects.html) in its additional settings tab, this is activated by default.

Then attempt to bake the Scene.