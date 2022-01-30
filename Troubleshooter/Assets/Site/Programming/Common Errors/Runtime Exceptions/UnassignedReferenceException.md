## UnassignedReferenceException
### Description
An UnassignedReferenceException is a type of [NullReferenceException](NullReferenceException.md) where an Object field has not been assigned to.  

### Resolution
Assign a value to the field via the Inspector.  

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/inspector-references.webm"></video>

If the component is assigned in the Inspector, search the Scene (`t:ExampleComponent`), ensuring there aren't duplicates causing the issue.  
Logs can also be made to ping objects they reference using the [context parameter](../../Debugging/Logging/Logging%20How-to.md).