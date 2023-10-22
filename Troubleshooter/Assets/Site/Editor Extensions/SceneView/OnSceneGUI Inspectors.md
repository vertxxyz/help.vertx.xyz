## OnSceneGUI not called: Inspectors

### Primary inspector
OnSceneGUI may only be called for the *primary* Inspector. This is a concept Unity does not expose to the user.  
When opening Inspectors, the first one opened becomes the primary one. Opening an object in this Inspector may show OnSceneGUI, while others may not.  

Reopening your Inspectors may fix the issue. Resetting the window layout (using the Layouts dropdown in the top right) may also do so.  

### Debug mode
It's also worth noting that an inspector in [Debug mode](https://docs.unity3d.com/Manual/InspectorOptions.html) does not receive the callbacks, as the custom Editor isn't used.  

