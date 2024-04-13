# NullReferenceException: Debugging
Now we understand [reference types](Reference%20Types.md) and when they are being [accessed](Access.md), we can debug to find which one is `null`.

Previous knowledge may make this step unnecessary, but if you have multiple access operators on one line, or aren't entirely confident, you need to check by debugging!

## Checking with logs

When logging to find null values it's important to make individual logs for each access, or else the log will also throw an exception and not be printed.

<<Code/Logging/Logging 3.rtf>>

Null values will sometimes print nothing, so note if a log does not print details, it could be a `null` value.

[Learn more.](../../Debugging/Logging/How-to.md)

## Checking with the debugger

Using the debugger is a fast and thorough way of exploring your code's state, where code execution is halted and values can be inspected directly.  

Checking for `null` requires placing a ![Rider breakpoint](../../Debugging/breakpoint_dark.svg){.inline} breakpoint on that line, and ![Rider attach](../../Debugging/debug_dark.svg){.inline} attaching the debugger. When the breakpoint is hit you can hover to inspect your variables. Stop the debugger to continue normal execution.  

[Learn more.](../../Debugging/Debugger.md)

---

[I understand which variable is `null`.](Options.md)
