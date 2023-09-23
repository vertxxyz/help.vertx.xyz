## Physics queries
Physics queries include casts, overlaps, and checks in the [`Physics`](https://docs.unity3d.com/ScriptReference/Physics.html) or [`Physics2D`](https://docs.unity3d.com/ScriptReference/Physics2D.html) classes.

### Test whether your query hits an object
Test that a physics query hits an object by using a [breakpoint](Debugging/Debugger.md) or [log](Debugging/Logging/How-to.md).  
:::warning{.small}  
Don't put breakpoints or logs **after** or **inside** other code when testing a method runs.  
:::

:::note
#### My query is not hitting an object
- [I am using a layer mask to filter my query.](Physics%20Queries/Layer%20Masks.md)
- [I am not using a layer mask.](Physics%20Queries/Ignore%20Raycast.md)

:::  
:::note
#### I am receiving a message but no logic runs
You likely have an error in your logic. Common mistakes include:
- Comparing tags using equality; use [`CompareTag`](https://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html) to get proper warnings.
- Destroying components instead of their [`gameObject`](https://docs.unity3d.com/ScriptReference/Component-gameObject.html), leading to the appearance that nothing happens.
- Ignoring errors that throw exceptions, these cause the code after to not run (fix the exceptions to resolve the issue).

:::