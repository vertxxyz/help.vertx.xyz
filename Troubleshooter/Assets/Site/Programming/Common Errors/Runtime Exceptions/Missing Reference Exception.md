## Missing reference exception
### Description
A Missing Reference Exception is a type of [null reference exception](Null%20Reference%20Exception.md) where a Unity Object field was previously assigned to, and the contents have been deleted or become invalid.  
### Resolution
Re-assign a value to the field via the Inspector.

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/inspector-references.webm"></video>

---

This can also occur when trying to retrieve something that is not a `Component` using the [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) functions.  
For example, `There is no 'GameObject' attached to the "Example" game object`, is caused by `GetComponent<GameObject>()`.  
To get a GameObject from a component Unity exposes the [gameObject](https://docs.unity3d.com/ScriptReference/Component-gameObject.html) property.