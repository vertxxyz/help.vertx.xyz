## Basic debugging

> it doesn't work
> 😢 pls help!

When you have an issue, there are some common steps that are often applicable to every problem. Internalising these and making sure they're covered when you encounter problems, will allow you to quickly solve issues alone.

### Check the object setup is correct
If you are using MonoBehaviours, there are some basics that should be checked:
1. Make sure the component has been [added to a GameObject in the scene.](https://docs.unity3d.com/Manual/UsingComponents.html)
1. [Search the scene](../Scene%20View/Searching.md) to check for accidentally duplicated objects if they could interfere with your issue.

### Check the code you have written is actually being run
If you are writing code, make sure your code has compiled and is running.
1. Check the [Console window](https://docs.unity3d.com/Manual/Console.html) (**Window | General | Console**, <kbd>Ctrl+Shift+C</kbd>) for [compiler errors](../Editor/Compiler%20Errors.md).
1. Check that you have actually saved your code.
1. Do some [debugging](../Debugging/Debugger.md) or [logging](../Debugging/Logging/How-to.md) to check the state is what you expect.

### Inspect the problem
There are powerful tools that will help you get a detailed look at problems. Learn them, as even cursory understanding of their output can produce valuable insights.  
- For code proboems, use the [Debugger](../Debugging/Debugger.md).  
- For performance issues, the [Profiler](https://docs.unity3d.com/Manual/Profiler.html) is invaluable.
- For graphical issues, the [Frame Debugger](https://docs.unity3d.com/Manual/frame-debugger-window.html) can sometimes help.
- For issues with UI Toolkit, there's the [UI Toolkit Debugger](https://docs.unity3d.com/Manual/UIE-ui-debugger.html) and the [Event Debugger](../UI%20Toolkit/Input%20Issues.md).

### Visualise the problem
Use [drawing functions](../Debugging/Draw%20Functions.md) if the problem can be displayed physically. Compare the drawn results to what you expect, and look into any discrepancies.

### Narrow down the problem
Check whether the issue can be recreated in an empty scene, reduce the amount of variables to narrow down the issue. If the problem is fixed in another scene, try disabling objects in the main scene to narrow down the problem.  

If you use source control, go back and find where the issue began, and compare what's changed. If you don't use source control, consider it!
For issues manifesting over the course of a long period, Git has tools like Bisect to narrow down the problematic commit.

### Check that the editor or project state are not the issue
Sometimes, things the editor itself can break, or the data generated from importing assets or scripts can become invalidated or incorrect. You must be aware of it to avoid endlessly chasing non-existent problems.
1. Restarting Unity may fix issues, it's a simple place to start.
1. If the issue persists, try a [project reimport](../Script%20Loading%20Issues/Project%20Reimport.md) (deleting the Library folder).

### Consider updating Unity
If you're experiencing issues that are unrelated to your work, like crashes, visual bugs, or editor issues,
consider updating your project to the latest patch release for your Unity version.


---
[Check other resources on this site.](../Index.md)
