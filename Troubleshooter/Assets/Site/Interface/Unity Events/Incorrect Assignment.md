## Incorrect Assignment to UnityEvents
### Description
Events should have an appropriate **Component** instance assigned to their object field. If you are seeing MonoScript you have assigned a Script Asset from the Project window.

### Resolution
Assign a Component to the field. Add the target script to a GameObject before dragging it or the GameObject it is attached to into the field. See [Adding Components](https://docs.unity3d.com/Manual/UsingComponents.html) for more information.  
The object assigned to the field must be, or attached to, the GameObject with the target Component.  

<video width="750" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/unity-event-references.webm"></video>