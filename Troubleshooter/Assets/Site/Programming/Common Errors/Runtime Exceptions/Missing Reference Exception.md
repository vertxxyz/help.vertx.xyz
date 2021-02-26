### Missing Reference Exception
A Missing Reference Exception is a type [Null Reference Exception](Null%20Reference%20Exception.md) where a Unity Object field was previously assigned to, and the contents have been deleted or become invalid.  

--- 

This can also occur when trying to retrieve something that is not a `Component` using the `GetComponent` functions.  
For example, if the error mentioned `GameObject` directly, `There is no 'GameObject' attached to the "X" game object`, then `GetComponent<GameObject>` or a variation on it has populated that variable.  
To get a GameObject from a component Unity exposes the `gameObject` property.