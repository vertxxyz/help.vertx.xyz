<<Abbreviations/NRE.md>>
## Logging: How-to
### Usage
Unity's [Debug.Log](https://docs.unity3d.com/ScriptReference/Debug.Log.html) function will print a message to the [Console window](https://docs.unity3d.com/Manual/Console.html).  
Code can pass any object to the log and it will be converted to a `string` and displayed.  

When logging it is important to place your log **behind** any error that can occur, as exceptions will halt execution.
<<Code/Logging/Logging 1.rtf>>

### The context parameter
The second parameter of `Debug.Log`, the context object, is extremely valuable. This `UnityEngine.Object` will be **pinged** when the log is selected. This is often used to discern whether there are multiple instances of a script producing a log.

<<Code/Logging/Logging 2.rtf>>

### Null references

When dealing with null it's accessing the `null` that will throw an exception, so if a long line is causing an NRE it's important to make individual logs for each access, or else the log will also throw an exception and not be printed.

<<Code/Logging/Logging 3.rtf>>

Null values will sometimes print nothing, so note if a log does not print details, it could be a `null` value.  

A much less tedious way of discovering what values are null is to [use the debugger](../Debugger.md), where code execution is halted and values can be inspected directly.

### Extra details
The `ToString` implementation for vectors have very little precision, so when logging it is best to use `.ToString("F7")` to display a suitable amount of decimal places.  

The [print](https://docs.unity3d.com/ScriptReference/MonoBehaviour-print.html) function is only inherited from `MonoBehaviour` and indirectly calls `Debug.Log`. It also does not take the context parameter, and so should generally be avoided.  

Unity has multiple types of logs:  
:::info{.inline}
Log can describe informative or verbose details.
:::  
:::warning{.inline}
Warning is for non-critical failures, like handled issues or basic misconfiguration.
:::  
:::error{.inline}
Error or Exception are for failures that must be addressed, or halt code execution.
:::  