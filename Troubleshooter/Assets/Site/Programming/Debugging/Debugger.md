### Debugger
#### Description
The debugger is a tool that allows the halting of program execution so values can be inspected, lines of code can be stepped over, and sometimes even modified.  
This does not require recompiling your code, or exiting play mode.

A functioning IDE is required, so first check [IDE configuration](../IDE%20Configuration.md) if you are experiencing basic issues.

#### Shared Concepts
##### Attaching
Your IDE needs to target a running application to start a debugging session. Usually this functionality is found in one of the top utility bars in an IDE's interface.

##### Breakpoints
Breakpoints are the entry-point to a debugging session, mark a line at or before a problem area and when the debugger is attached execution will halt when this line is reached.  

When program execution is halted **Unity will freeze**, you can now use the other debugger features of your IDE to assess problems. Stop debugging to resume Unity's normal function; debuggers will have a Resume Program button which will continue whilst remaining attached.

##### Variable Inspection
Hovering over an initialised variable during debugging will provide you with a view of its internals. This enables you to discover information about faults in logic, or uninitialised values.  
Often a debugger will also allow you to hover an expression to evaluate its outcome (eg. the result of an if statement).

##### Stepping
Stepping through code is a way to continue execution line by line, optionally stepping into or over functions and properties. This gives information about program execution, testing false assumptions about branching behaviour or loops.

##### Pausing
Execution can be manually halted similar to a breakpoint, this is very helpful when debugging freezes caused by infinite loops. Just pause execution when the freeze occurs, and the debugger should lead you to the relevant section of code.

#### Usage
##### Visual Studio
VS has a great rundown of their debugger found [here](https://docs.microsoft.com/en-us/visualstudio/debugger/debugger-feature-tour). Instructions about configuration are not specific to Unity and can be ignored.


##### Visual Studio Code
VSC does not come with a debugger built-in, and [the extension](https://marketplace.visualstudio.com/items?itemName=Unity.unity-debug) needs to be manually installed.  
How-to instructions for debugging can be found [here](https://code.visualstudio.com/docs/editor/debugging) with Debug actions, and Breakpoints being the most relevant information. Instructions about configuration are not specific to Unity and can be ignored.

##### JetBrains Rider
Rider has detailed information about debugging Unity applications [here](https://www.jetbrains.com/help/rider/Debugging_Unity_Applications.html) and [here](https://www.jetbrains.com/help/rider/Using_Breakpoints.html).  
As of Rider 2020.2 rider also has [Pausepoints](https://blog.jetbrains.com/dotnet/2020/06/11/introducing-unity-pausepoints-for-rider/), the ability to pause (`Debug.Break()`) Unity at the end of a frame once a Pausepoint is hit.

#### Debugging Builds
Builds require *Development Build* and *Script Debugging* to be enabled in the build settings (**File > Build Settings**) to debug script code. When attaching the debugger attach to the built Player and not the Unity Editor.  
More information can be found [here](https://docs.unity3d.com/Manual/ManagedCodeDebugging.html), including the debugging of mobile devices.  