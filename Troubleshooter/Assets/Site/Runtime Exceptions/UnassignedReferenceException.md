## UnassignedReferenceException

An UnassignedReferenceException is a type of [NullReferenceException](NullReferenceException.md) where an Object field has not been assigned to.

### Resolution
Assign a value to the field via the Inspector to the object mentioned in the [stack trace](../Stack%20Traces.md).

<video width="750" height="200" autoplay loop muted controls><source type="video/webm" src="https://unity.huh.how/Video/inspector-references.webm"></video>

#### I can't find the object with an unassigned value
[Search the Scene](../Scene%20View/Searching.md) (`t:ExampleComponent` for example) after the error occurs (don't exit Play Mode), ensuring there aren't other components causing the issue. Double-check GameObjects for multiples of the same component.  

If you still cannot find the object, use the [debugger](../Debugging/Debugger.md); or add a log before the exception, pinging the object with the [context parameter](../Debugging/Logging/How-to.md).
