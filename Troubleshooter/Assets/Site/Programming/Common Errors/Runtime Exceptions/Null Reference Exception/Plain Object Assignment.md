## Null Reference Exception: Plain C# Objects
- **Either:**  
  Assign a reference to the variable (choose one)
    - [Serialize the field](../../../Variables/Serialization/Serialization%201/Serializing%20A%20Field%201.md) if appropriate.
    - Assign the reference using [`new`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator). This can be done inline or in a method like `Awake` or `Start`.

  **Or:**  
  Check the reference is not null before trying to access it  
  `if (example != null)`
- Ensure that nothing sets it to `null` before you attempt to use it.