## The debugger

The debugger is a tool that can halt code execution. Values can be inspected, lines of code can be stepped over, and sometimes even modified.  
This does not require recompiling your code, or exiting Play Mode.

A functioning IDE is required, so first check [IDE configuration](../IDE%20Configuration.md) if you are experiencing basic issues.

### ![Rider attach](debug_dark.svg) Attaching
Your IDE needs to target a running application to start debugging. Usually this functionality is found in one of the top utility bars in an IDE's interface.

^^^
![Attaching Rider's debugger](debugger-attach-rider.png)
^^^ Attaching Rider to the Unity Editor

### ![Rider breakpoint](breakpoint_dark.svg) Breakpoints
Breakpoints are the entry point to a debugging session. Mark a line with a breakpoint and execution will halt when this line is reached if the debugger is attached.  

^^^
![Attaching Rider's debugger](breakpoint-rider.png)
^^^ A breakpoint in Rider

When execution is halted **Unity will freeze**, this is normal. You can now use the other debugger features of your IDE to assess problems. ![Rider stop debugging](stop_dark.svg){.inline} **stop** debugging to resume Unity's normal function, or ![Rider stop debugging](resume_dark.svg){.inline} **resume** to continue execution while remaining attached.

#### Managing breakpoints
Select the gutter for an executable line to add a breakpoint. Repeat the selection to remove it.  
You can ![Rider all breakpoints](multipleBreakpoints_dark.svg){.inline} **view all breakpoints** to manage them in bulk, removing them all at once, or just finding forgotten ones.

#### Muting breakpoints
Often you want to continue testing with the debugger connected, without triggering breakpoints. You can ![Rider mute breakpoints](muteBreakpoints_dark.svg){.inline} **mute breakpoints** to stop them triggering without removing them. Individual breakpoints can also be ![Rider disabled breakpoint](breakpointDisabled_dark.svg){.inline} **disabled**.

#### Conditional breakpoints
Right-click a breakpoint and after adding a condition based on in-scope variables the breakpoint will become a ![Conditional breakpoint in Rider](breakpointConditional_dark.svg){.inline} **conditional breakpoint**, and only trigger when the condition is met.

^^^
![Conditional breakpoint in Rider](conditional-breakpoint-rider.png)
^^^A conditional breakpoint in Rider

#### Tracepoints (logging breakpoints)
Disable the breakpoint's suspend execution setting in the right-click menu so it becomes a ![Unsuspended breakpoint in Rider](breakpointUnsuspendent_dark.svg){.inline} **tracepoint**, navigate to more settings and add logging. The logs will print to the **IDE's debug console**, not Unity. 
This makes a great substitution for [manual logging](Logging/How-to.md), avoiding unnecessary recompilation. 

### ![Rider data](binaryData_dark.svg) Variable inspection
Hovering over an initialised variable during debugging will provide you with a view of its internals. This lets you discover faulty logic or uninitialised values.  
Often a debugger will also allow you to hover an expression to evaluate its outcome (the result of an if statement for example).

^^^
![Variable inspection in Rider](variable-inspection-rider.png)
^^^ Inspecting a variable using Rider's debugger

### ![Rider step over](stepOver.svg) Stepping
Stepping through code is a way to continue execution line by line, optionally stepping into or over functions and properties. This gives information about program execution, testing false assumptions about branching behaviour or loops.

| Name                                           | Description                                               |
|------------------------------------------------|-----------------------------------------------------------|
| ![Step over](stepOver.svg){.inline} Step over  | Execute the next line of code without entering functions. |
| ![Step into](stepInto.svg){.inline} Step into  | Execute the next line of code and enter any functions.    |
| ![Step out](stepOut.svg){.inline} Step out     | Execute until the function is exited.                     |

### ![Rider pause](pause_dark.svg) Pausing
Execution can be manually halted similar to a breakpoint, this is very helpful when debugging freezes caused by infinite loops. Just pause execution when the freeze occurs, and the debugger should lead you to the relevant section of code.

## Usage
### ![VS](/Images/visualstudio.svg) Visual Studio
VS has a great rundown of their debugger found [here](https://docs.microsoft.com/en-us/visualstudio/debugger/debugger-feature-tour). Instructions about configuration are not specific to Unity and can be ignored.  
If you're looking for video tutorials, [this video series](https://www.youtube.com/playlist?list=PLReL099Y5nRdW8KEd59B5KkGeqWFao34n) is a fantastic overview of using the debugger with Unity.

### ![VS Code](/Images/vscode.svg) Visual Studio Code
VSC does not come with a debugger built-in, and [the extension](https://marketplace.visualstudio.com/items?itemName=Unity.unity-debug) needs to be manually installed.  
How-to instructions for debugging can be found [here](https://code.visualstudio.com/docs/editor/debugging) with Debug actions, and Breakpoints being the most relevant information. Instructions about configuration are not specific to Unity and can be ignored.

### ![VS](/Images/jetbrains_rider.svg) JetBrains Rider
Rider has detailed information about debugging Unity applications [here](https://www.jetbrains.com/help/rider/Debugging_Unity_Applications.html) and [here](https://www.jetbrains.com/help/rider/Using_Breakpoints.html).  
As of Rider 2020.2 rider also has [Pausepoints](https://blog.jetbrains.com/dotnet/2020/06/11/introducing-unity-pausepoints-for-rider/), the ability to pause (`Debug.Break()`) Unity at the end of a frame once a Pausepoint is hit.

## Debugging builds
Builds require **Development Build** and **Script Debugging** to be enabled in the build settings (**File | Build Settings**) to debug script code. When attaching the debugger attach to the built Player and not the Unity Editor.  
More information can be found [here](https://docs.unity3d.com/Manual/ManagedCodeDebugging.html), including the debugging of mobile devices.  