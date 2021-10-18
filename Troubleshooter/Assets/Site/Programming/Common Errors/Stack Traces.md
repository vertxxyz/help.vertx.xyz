## Stack traces
### Description
Stack traces are reports of the path code took to get to an execution point.  
Compiler errors and runtime exceptions will generally be accompanied with a stack trace.  
Stack traces are vital tools that give context to errors, and are the first step to understanding cause.  

:::info
If stack traces are not appearing, change the **Stack Trace Logging**[^1] setting in the Console window's context menu back to **Script Only**.  
:::
[^1]: This setting is shared in **Edit | Project Settings | Player**, and also effects log details in builds.

### Usage
:::code-context
:::error
NullReferenceException: Object reference not set to an instance of an object  
::Example.Foo ()::{.context-a} (at Assets/Scripts/::Example.cs::{.context-b}:::14::{.context-c} )
:::
:::  

The key information to look for in a stack trace is the ::file name::{.context-b}, ::method name::{.context-a}, and ::line number::{.context-c}.  
Information is presented newest to oldest, the path the code took to get to here.  

### Details

:::editor-colors
:::code-context
UnassignedReferenceException: The variable pivot of Rotator has not been assigned.  
You probably need to assign the pivot variable of the GeneralRotationTest script in the inspector.  
::UnityEngine.Transform.get_rotation ()::{.context-a} (at ::&lt;cc54933d6df84ff08e916a4036d0b6c6&gt;:0::{.context-d})  
::Rotator.Update ()::{.context-a} (at Assets/Scripts/::Rotator.cs::{.context-b}:::23::{.context-c})  
:::
:::  

Stack traces from DLLs or native locations may show ::a hash and no line number::{.context-d}. As you will not be editing these files, this can be ignored. Look at the last location in user code when troubleshooting.  

If a method starts with `get_` or `set_` then it is a property with a name matching the suffix.  