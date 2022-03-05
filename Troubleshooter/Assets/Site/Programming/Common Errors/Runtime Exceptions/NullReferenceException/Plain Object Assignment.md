## NullReferenceException: Plain C# Objects
To resolve a `NullReferenceException` caused by a `null` `object` that isn't a `UnityEngine.Object` type you can choose one of the following options:

**Either:**  
:::note
#### Assign a reference to the variable (choose one)  
- [Serialize the field](../../../Variables/Serialization/Serialization%201/Serializing%20A%20Field%201.md) if appropriate.
- Assign the reference using [`new`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator). This can be done inline or in a method like `Awake` or `Start`.
:::
**Or:**  
:::note
#### Check the reference isn't null before trying to access it (choose one)  
- Exit early if null.
    ```csharp
    if (example == null)
    {
        return; // Exit early
    }
    ```
- Nest your code in a null check.
    ```csharp
    if (example != null)
    {
        // Code that uses example here
    }
    ```
:::
**Or:**  
:::note
#### Ensure that nothing sets it to `null` before you attempt to use it.
It must be assigned or have a value serialized to begin with.
:::