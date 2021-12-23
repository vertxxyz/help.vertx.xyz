:::code-context
:::error
NullReferenceException: Object reference not set to an instance of an object  
::Example.Foo ()::{.context-a} (at Assets/Scripts/::Example.cs::{.context-b}:::14::{.context-c} )
:::
:::

The key information to look for in a stack trace is the ::file name::{.context-b}, ::method name::{.context-a}, and ::line number::{.context-c}.  
Information is presented newest to oldest, the path the code took to get to here.  