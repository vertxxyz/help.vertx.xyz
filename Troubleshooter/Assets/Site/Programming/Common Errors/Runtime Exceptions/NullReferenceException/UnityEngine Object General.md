## NullReferenceException: UnityEngine.Object — General

To resolve a `NullReferenceException` caused by a `null` `UnityEngine.Object` you can choose one of the following options:

### First
:::note
#### Assign a reference to the variable (choose one)
- [Serialize a reference](../../../References/Serializing%20Component%20References.md), and ensure it is assigned in the component's inspector.
- Manually assign the reference in a method like `Awake` or `Start` using [`GetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) or similar.
- Initialise the variable with [`AddComponent`](https://docs.unity3d.com/ScriptReference/GameObject.AddComponent.html) or [`CreateInstance`](https://docs.unity3d.com/ScriptReference/ScriptableObject.CreateInstance.html).
:::  
### Then
:::note
#### 1. Assign all references (if you serialized a reference)
Check that all references have been [assigned in the inspector](../../../References/Serializing%20Component%20References.md).
^^^
<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://unity.huh.how/Video/inspector-references.webm"></video>
^^^ Assigning a serialized reference

#### Check for duplicate objects
Search the Scene (`t:ExampleComponent` for example) when the error occurs, ensuring there aren't duplicate components causing the issue.  
Logs can also be made to ping objects they reference using the [context parameter](../../../Debugging/Logging/How-to.md), this helps find the exact object.  
:::

:::note
#### 2. Do not override the reference
- Remove any assignments that could be overriding your reference.  
  A common mistake is missing `GetComponent` setting a serialized value to `null` in `Awake` or `Start`.
- Ensure nothing is destroying the Object, or setting it to `null` before you attempt to use it.

:::  
:::note
#### 3. Check assignments use correct assumptions
Some examples:
- `GetComponent<Example>()` will return `null` if an `Example` component isn't attached to the same GameObject.  
  A common example of this being wrong is `Text` when using subtypes of `TMP_Text`.
- `Camera.main` will return null if there isn't a camera tagged as <kbd>MainCamera</kbd>.
:::

:::note
#### 4. Don't try to use the object before it's assigned
Assignment must occur before access. Often you would use [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) to get, and [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html) to use.
:::

:::note
#### 5. Do not use a modern null-checking operator
Check that you are not using modern null-checking operator (`?.`, `??`, `??=`).  
See [Unity null](../../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.  
:::