## Occlusion setup
Occlusion culling is made up of both occluders and occludees.  
Occluders block visibility, and occludees are visible based on them.

### Resolution
Selectively mark occluders as occluder static in the **Object** tab of the **Occlusion window**.  
Occludees can be marked as Occludee Static in the same window, this is for objects that don't move.    
The Scene Hierarchy can be filtered to contain only renderers, which sometimes helps with this process.  
The renderers of dynamic, moving occludees can be marked as having [dynamic occlusion](https://docs.unity3d.com/Manual/occlusion-culling-dynamic-gameobjects.html) in its additional settings tab, this is activated by default.

Then attempt to bake the Scene.