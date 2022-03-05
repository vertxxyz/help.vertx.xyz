<<Abbreviations/IDE.md>>
## Compiler Errors
### Description
Compiler errors describe issues with code that will stop it from being compiled and used.  
These errors are accompanied with [warning messages](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/), error codes that detail what failed.  

### Usage
When assessing compiler errors, clear the console and work top-down to resolve the first reported error before all others.  

#### Overview

:::code-context
:::error
Assets/Scripts/::Example.cs::{.context-a}::(::21::{.context-b},::40::{.context-c}): error ::CS1001::{.context-d}: ::Identifier expected::{.context-e}
:::
:::  

The ::file name::{.context-a}, ::line number::{.context-b}, ::column number::{.context-c}, ::error code::{.context-d}, and ::description::{.context-e} are the key pieces of information to look for.  

::file name::{.context-a}, the file containing the code that has the compiler error.  
::line numbers::{.context-b} are found in the gutter of your IDE (the left-hand column beside your code).  
::column numbers::{.context-c} are usually found in the bottom right of the IDE's status bar, in the format `line number:column number`.  
::error codes::{.context-d} are the shortest description of the issue with a code block. Using a search engine or [MSDN](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/) with this code will provide information to resolve the issue.  
::descriptions::{.context-e} provide important context to the error code, explaining what specifically may be wrong.

:::info
If your error isn't ::underlined in red::{.error-underline} in your IDE, [configure it correctly](../../IDE%20Configuration.md).  
Compiler errors should be underlined, and you should see autocompleted suggestions as you type.
:::

### Details
#### Errors in packages
If errors are appearing from a package, and not Assets, then either the package isn't valid for your version of Unity, or it is corrupted.  
Go to the Package Manager (**Window | Package Manager**), expand the dropdown for the package having issues, and [upgrade or downgrade versions](https://docs.unity3d.com/Manual/upm-ui-update.html) until you no longer have errors in the project.