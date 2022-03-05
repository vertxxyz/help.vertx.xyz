## [IndexOutOfRangeException](https://docs.microsoft.com/en-us/dotnet/api/system.indexoutofrangeexception)
### Description
Collections are accessed via indices using the indexer syntax: `[value]`.  
C#'s indices are **zero-indexed**. This means that indices begin at 0, and go up to but don't include the length of the collection.  
`[0..Length)`  

### Resolution
Ensure that the line pointed to by the [stack trace](../Stack%20Traces.md) is accessing an index that is within the limits of the collection.  
Common mistakes are:
- Accessing an empty collection (`.Length` or `.Count` is `0`).
- An improperly written `for` or `while` loop.

You can use the [debugger](../../Debugging/Debugger.md) to step over your code, inspecting variables and execution to assess what is wrong.  

### Notes
A [functioning IDE](../../IDE%20Configuration.md) will be able to autocomplete `for` loops by typing <kbd>for</kbd> and pressing tab/enter.  
Reverse `for` loops can be autocompleted with <kbd>forr</kbd>. Doing this helps prevent basic mistakes.  