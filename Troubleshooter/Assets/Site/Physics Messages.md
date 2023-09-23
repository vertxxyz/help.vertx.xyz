## Physics messages
Physics messages are sent between [`Physics`](https://docs.unity3d.com/ScriptReference/Physics.html) or [`Physics2D`](https://docs.unity3d.com/ScriptReference/Physics2D.html) bodies when their colliders interact.

### Test whether messages are sent to your objects
Test that you receive a physics message by using a [breakpoint](Debugging/Debugger.md) or [log](Debugging/Logging/How-to.md).  
:::warning{.small}  
Don't put breakpoints or logs **after** or **inside** other code when testing a method runs.  
:::

:::note
#### I am not receiving a message
- [I am working in 3D.](Physics%20Messages/1%203D%20Physics%20Messages.md)
- [I am working in 2D.](Physics%20Messages/1%202D%20Physics%20Messages.md)

:::  
:::note
#### I am receiving a message but no logic runs
You likely have an error in your logic. Common mistakes include:
- Comparing tags using equality; use [`CompareTag`](https://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html) to get proper warnings.
- Destroying components instead of their [`gameObject`](https://docs.unity3d.com/ScriptReference/Component-gameObject.html), leading to the appearance that nothing happens.
- Ignoring errors that throw exceptions, these cause the code after to not run (fix the exceptions to resolve the issue).

:::