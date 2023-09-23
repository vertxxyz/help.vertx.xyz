<<Abbreviations/IDE.md>>
## Stack traces

Stack traces are reports of the path code took to get to an execution point.
Compiler errors and runtime exceptions will generally be accompanied with a stack trace.
Stack traces are vital tools that give context to errors, and are the first step to understanding cause.

:::info
If stack traces are not appearing, change the **Stack Trace Logging**[^1] setting in the Console window's context menu back to **Script Only**.
:::
[^1]: This setting is shared in **Edit | Project Settings | Player**, and also effects log details in builds.

### Usage
Click on an error in the Console to see the full stack trace.

<<Programming/Common Errors/Stack Trace Breakdown.md>>

#### Error locations
You can usually double-click an error to be taken to the location, but this may not always be correct. Code changes can cause a location mismatch, as can miscellaneous reporting issues. Errors may also be thrown deeper than the cause itself, for example an [ArgumentException](../Runtime%20Exceptions/ArgumentException.md) may be reported inside a method, when it's the invalid argument that matters.
If you have a runtime error, you can [debug the issue](../Debugging.md) to narrow down the issue further.

### Details
#### Compiler errors
:::code-context
:::error
Assets\Scripts\\::Example.cs::{.context-b}(::80::{.context-c},::9::{.context-d}): error CS1002: ; expected
:::
:::

Compiler errors include a ::column number::{.context-d}, but don't include the method name.
Most IDEs will display the line and column number of the current selection in the bottom right as `line:column`.

#### Traces from native code
:::code-container{.code-container-inner}
:::code-context
UnassignedReferenceException: The variable pivot of Rotator has not been assigned.
You probably need to assign the pivot variable of the GeneralRotationTest script in the inspector.
::UnityEngine.Transform.get_rotation ()::{.context-a} (at ::&lt;cc54933d6df84ff08e916a4036d0b6c6&gt;:0::{.context-d})
::Rotator.Update ()::{.context-a} (at Assets/Scripts/::Rotator.cs::{.context-b}:::23::{.context-c})
:::
:::

Stack traces from DLLs or native locations may show ::a hash and no line number::{.context-d}. As you will not be editing these files, this can be ignored. Look at the last location in user code when troubleshooting.

#### Properties
If a method starts with `get_` or `set_` then it is a property with a name matching the suffix.
