## Basic debugging

> It doesn't work.  
> pls help!

### Using MonoBehaviours
1. [Add the component to a GameObject in the scene.](https://docs.unity3d.com/Manual/UsingComponents.html)
1. [Search the scene](../Interface/Scene%20View/Searching.md) to check for accidentally duplicated objects if they could interfere with your issue.

### General
1. Check the [Console window](https://docs.unity3d.com/Manual/Console.html) (**Window | General | Console**, <kbd>Ctrl+Shift+C</kbd>) for [compiler errors](../Editor/Compiler%20Errors.md).
1. Check that you have actually saved your code.
1. Do some [debugging](../Programming/Debugging/Debugger.md) or [logging](../Programming/Debugging/Logging/How-to.md) to see what's running and that the state is what you expect.
1. Use [drawing functions](../Programming/Debugging/Draw%20Functions.md) if the problem can be visualised physically.
1. Often restarting Unity will fix issues.
1. If the issue persists, try a [project reimport](../Programming/Scripts/Loading%20Issues/Project%20Reimport.md) (deleting the Library folder).
1. [Check for other resources on this site.](../Main.md)