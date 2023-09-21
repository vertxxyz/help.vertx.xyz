## NullReferenceException: Debugging
Now we understand [reference types](Reference%20Types.md) and when they are being [accessed](Access.md), we can debug to find which one is `null`.  

When logging to find null values it's important to make individual logs for each access, or else the log will also throw an exception and not be printed.

<<Code/Logging/Logging 3.rtf>>

Null values will sometimes print nothing, so note if a log does not print details, it could be a `null` value.

:::info
A much less tedious way of discovering what values are null is to [use the debugger](../Programming/Debugging/Debugger.md), where code execution is halted and values can be inspected directly.
:::

---  

[I understand which variable is null.](Options.md)