## UnassignedReferenceException
### Description
An UnassignedReferenceException is a type of [NullReferenceException](NullReferenceException.md) where an Object field has not been assigned to.  

### Resolution
Assign a value to the field via the Inspector.  

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://unity.huh.how/Video/inspector-references.webm"></video>

If the object is assigned in the Inspector, [search the Scene](../../../Interface/Scene%20View/Searching.md) (`t:ExampleComponent` for example), ensuring there aren't duplicate components causing the issue.  
Logs can also be made to ping objects they reference using the [context parameter](../../Debugging/Logging/How-to.md).