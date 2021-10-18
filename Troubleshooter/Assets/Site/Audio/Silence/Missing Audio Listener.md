## Missing Audio Listener
There must be an AudioListener in the Scene.  
When searching `t:AudioListener` in the Hierarchy view in Play Mode there should be a single result.  
If there are no Audio Listeners then you need to add the component (generally they are present on the main Camera.)  
If you have multiple, this can cause other issues, removing the second listener is required.  

---
[Audio still cannot be heard](Audio%20Disabled.md)