## Physics messages

Test that you receive a physics message by using a [breakpoint](Debugging/Debugger.md) or [log](Debugging/Logging/How-to.md).  
:::warning{.inline}  
Don't put breakpoints or logs **after** or **inside** other code when testing a method runs.  
:::

:::note
#### I am not receiving a message
- [I am not receiving a Collision message.](Physics%20Messages/1%20Collision%20Messages.md)
- [I am not receiving a Trigger message.](Physics%20Messages/1%20Trigger%20Messages.md)

:::  
:::note
#### I am receiving a message but no logic runs
You likely have an error in your logic. Common mistakes include:  
- Comparing tags using equality; use [`CompareTag`](https://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html) to get proper warnings.
- Destroying components instead of their [`gameObject`](https://docs.unity3d.com/ScriptReference/Component-gameObject.html), leading to the appearance that nothing happens.
- Ignoring errors that throw exceptions, these cause the code after to not run (fix the exceptions to resolve the issue).

:::