<<Abbreviations/NRE.md>>
# Logging: How-to
## Usage
Unity's [`Debug.Log`](https://docs.unity3d.com/ScriptReference/Debug.Log.html) function will print a message to the [Console window](https://docs.unity3d.com/Manual/Console.html) (**Window | General | Console**, <kbd>Ctrl+Shift+C</kbd>).
Code can pass any object to the log and it will be converted to a `string` and displayed.

:::note
<<General/Console Window Short.md>>

Be aware when **Collapse** is enabled in the same toolbar, which will merge identical logs.
:::

### String interpolation
You can use [string interpolation](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) to easily insert values into text:

<<Code/Logging/Logging 0.html>>

### Exceptions
When logging it is important to place your log **behind** any error that can occur, as exceptions will halt execution.
<<Code/Logging/Logging 1.rtf>>

### The context parameter
The **second** parameter, `context`, is extremely valuable. This object will be **pinged** in the Hierarchy or Project window when the log is selected in the Console window.

```csharp
Debug.Log(..., gameObject);
```

You can use this to check whether there are multiple instances of a script producing a log.

If no object is pinged it generally means it no longer exists in the Hierarchy or Project window. This usually means it was just destroyed.

## Null references

When dealing with null it's [access](../../Runtime%20Exceptions/NullReferenceException/Access.md) that will throw an exception, so if a long line is causing an NRE it's important to make individual logs for each access, or else the log will also throw an exception and not be printed.

<<Code/Logging/Logging 3.rtf>>

Null values will sometimes print nothing, so note if a log does not print details, it could be a `null` value.

A much less tedious way of discovering what values are null is to [use the debugger](../Debugger.md), where code execution is halted and values can be inspected directly.

See [`NullReferenceException`](../../Runtime%20Exceptions/NullReferenceException.md) for more information.

## Extra details
### Vectors
The [`ToString`](https://docs.unity3d.com/ScriptReference/Vector3.ToString.html) implementation for vectors shows very little precision, when logging you should use a format string to show an appropriate number of decimal places. See [vector equality](../../Vectors/Equality.md) for more information.
### Print
The [`print`](https://docs.unity3d.com/ScriptReference/MonoBehaviour-print.html) function is only inherited from `MonoBehaviour` and indirectly calls `Debug.Log`. It also does not take the context parameter, and so should generally be avoided.
### Logs, Warnings, and Errors
Unity has multiple types of logs:
:::info{.small}
Log can describe informative or verbose details.
:::
:::warning{.small}
Warning is for non-critical failures, like handled issues or basic misconfiguration.
:::
:::error{.small}
Error or Exception are for failures that must be addressed, or halt code execution.
:::

### HideInCallstack
You can use the [`[HideInCallstack]`](https://docs.unity3d.com/ScriptReference/HideInCallstackAttribute.html) attribute to mark methods to be hidden when **⋮ | Strip Logging Callstack** is enabled via the Console window menu. This is useful when creating your own wrappers for logging.
