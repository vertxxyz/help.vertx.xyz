## NullReferenceException: Debugging
When logging to inspect variables in a line of code throwing an NRE it's important to make individual logs for each access, or else the log will also throw an exception and not be printed.

<<Code/Logging/Logging 3.rtf>>

Null values will sometimes print nothing, so note if a log does not print details, it could be a `null` value.

A much less tedious way of discovering what values are null is to [use the debugger](../../../Debugging/Debugger.md), where code execution is halted and values can be inspected directly.

---  

[I understand which variable is null.](Options.md)