---
title: "Troubleshooting physics messages"
description: "Steps to debug Unity physics callbacks."
image: "physics-messages.png"
---

# Physics messages
Physics messages are sent between [`Physics`](https://docs.unity3d.com/ScriptReference/Physics.html) or [`Physics2D`](https://docs.unity3d.com/ScriptReference/Physics2D.html) bodies when their colliders interact.

## Test whether messages are sent to your objects
Test that you receive a physics message by using a [breakpoint](Debugging/Debugger.md) or [log](Debugging/Logging/How-to.md).  
:::warning{.small}  
Don't put breakpoints or logs **after** or **inside** other code when testing a method runs.  
:::

:::note
### I am not receiving a message
- [I am working in 3D.](Physics%20Messages/1%203D%20Physics%20Messages.md)
- [I am working in 2D.](Physics%20Messages/1%202D%20Physics%20Messages.md)

:::  
:::note
### I am receiving a message but no logic runs
You likely have an error in your logic. Common mistakes include:
- Comparing tags using equality; use [`CompareTag`](https://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html) to get proper warnings.
- Destroying components instead of their [`gameObject`](https://docs.unity3d.com/ScriptReference/Component-gameObject.html), leading to the appearance that nothing happens.
- Ignoring errors that throw exceptions, these cause the code after to not run (fix the exceptions to resolve the issue).
:::

:::note
### I am receiving a message twice
Both objects involved in a collision receive events.
If you only want one to run, compare the instance IDs of the objects involved in the collision using [`GetInstanceID`](https://docs.unity3d.com/ScriptReference/Object.GetInstanceID.html):

```csharp
// Exit early if this instance ID is less than the other object involved in the collision. 
if (GetInstanceId() < collider.GetInstanceID())
    return;
```
:::
