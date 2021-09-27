## Missing Audio Listener
Ensure that there is an AudioListener in the scene.  
When searching `t:AudioListener` in the Hierarchy view when playing there should only be one result.  
If there are no Audio Listeners then you need to add the component (generally they are present on the main Camera.)  
If you have multiple, this can cause other issues, removing the second listener is required.

---
[Audio still cannot be heard](Audio%20Disabled.md)