### Incorrect Assignment to Unity Events
#### Description:
Events should have a **Component** instance assigned to their object field. If you are seeing MonoScript you have assigned a Script Asset from the project view.  
#### Resolution:
Assign a Component to the field. You need to add your script to a GameObject before dragging it or the GameObject it is attached to into the field. See [Adding Components](https://docs.unity3d.com/Manual/UsingComponents.html) for more information.  

<video width="750" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/unity-event-references.webm"></video>
