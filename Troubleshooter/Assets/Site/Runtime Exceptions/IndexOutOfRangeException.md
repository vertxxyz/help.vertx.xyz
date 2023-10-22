## [IndexOutOfRangeException](https://docs.microsoft.com/en-us/dotnet/api/system.indexoutofrangeexception)

Collections are accessed via indices using the indexer syntax: `[value]`.
C#'s indices are **zero-indexed**. This means that indices begin at 0, and go up to but don't include the length of the collection.
`[0..Length)`

### Resolution
:::note
#### In local scopes
Ensure that the line pointed to by the [stack trace](../Stack%20Traces.md) is accessing an index that is within the limits of the collection.
The index needs to be 0 or above, and less than the length of the collection.
Common mistakes include:
- Accessing an empty collection (`.Length` or `.Count` is `0`).
- An improperly written `for` or `while` loop.
- Using an index from a different loop, `i` instead of `j` for example.

You can use the [debugger](../Debugging/Debugger.md) to step over your code, inspecting variables and execution to assess what is wrong.
:::

:::note
#### Inside lambdas
If you have code like this:
```csharp
for (int i = 0; i < values.Length; i++)
{
    values[i].onClick.AddListener(() => values[i].enabled = false);
}
```
The `i` in the delegate is not copied inside the for loop, it is created before it, reused, and increased as the counter of the loop. The value in the listener will increase to `values.Length` at the end of the loop.
To fix this, declare a local version of the counter that is used in the delegate:
```csharp
for (int i = 0; i < values.Length; i++)
{
    int iLocal = i;
    values[i].onClick.AddListener(() => values[iLocal].enabled = false);
}
```
See [anonymous methods and closures](../Programming/Specifics/Anonymous%20Methods%20and%20Closures.md) for more information.
:::

### Notes
A [functioning IDE](../IDE%20Configuration.md) can autocomplete `for` loops by typing <kbd>for</kbd> and pressing tab/enter.
Reverse `for` loops can be created with <kbd>forr</kbd>. This helps prevent basic mistakes.
