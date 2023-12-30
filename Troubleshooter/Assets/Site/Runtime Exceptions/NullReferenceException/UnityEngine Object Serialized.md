## NullReferenceException: UnityEngine.Object — Serialized
:::note
#### 1. Assign all references
Check that all references have been [assigned in the inspector](../../References/Serializing%20Component%20References.md).
^^^
<video width="750" height="200" autoplay loop muted controls><source type="video/webm" src="https://unity.huh.how/Video/inspector-references.webm"></video>
^^^ Assigning a serialized reference

#### Check for duplicate objects
[Search the Scene](../../Scene%20View/Searching.md) (`t:ExampleComponent` for example) when the error occurs, ensuring there aren't duplicate components causing the issue.
Logs can also be made to ping objects they reference using the [context parameter](../../Debugging/Logging/How-to.md), this helps find the exact object.
:::
:::note
#### 2. Do not override the reference
- Remove any `GetComponent` calls that could be overriding your reference.
   A common mistake is missing `GetComponent` setting a serialized value to `null` in `Awake` or `Start`.
- Ensure nothing is destroying the Object, or setting it to `null` before you attempt to use it.

:::
:::note
#### 3. Do not use a modern null-checking operator
Check that you are not using modern null-checking operator (`?.`, `??`, `??=`).
See [Unity null](../../Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.
:::
