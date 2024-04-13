# Incorrect assignment to UnityEvents

Events should have an appropriate **Component instance** assigned to their object field.  

If you are seeing `MonoScript` you have assigned a Script asset from the Project window.  
If you aren't seeing your type, then it may not be attached to the assigned object.

## Resolution
The target component must be assigned to the field, or it must be attached to the GameObject that is.

1. [Add the target component](https://docs.unity3d.com/Manual/UsingComponents.html) to a GameObject.  
1. Drag the GameObject into the UnityEvent's object field.

<video width="750" autoplay loop muted controls><source type="video/webm" src="https://unity.huh.how/Video/unity-event-references.webm"></video>

---

[The component still does not appear in the functions list.](Compiler%20Errors.md)
