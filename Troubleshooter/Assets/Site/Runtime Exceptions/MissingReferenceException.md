## MissingReferenceException

A MissingReferenceException is a type of [NullReferenceException](../../../NullReferenceException.md) where a Unity Object field was previously assigned to, and the contents have been deleted or become invalid.  
### Resolution
Re-assign a value to the field via the Inspector.

<video width="750" height="200" autoplay loop muted controls><source type="video/webm" src="https://unity.huh.how/Video/inspector-references.webm"></video>

---

This can also occur when trying to retrieve something that isn't a `Component` using the [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) functions.  
For example, `There is no 'GameObject' attached to the "Example" game object`, is caused by `GetComponent<GameObject>()`.  
To get a GameObject from a component Unity exposes the [gameObject](https://docs.unity3d.com/ScriptReference/Component-gameObject.html) property.