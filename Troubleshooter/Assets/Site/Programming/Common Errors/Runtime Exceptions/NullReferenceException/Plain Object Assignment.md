## NullReferenceException: Plain C# objects
To resolve a `NullReferenceException` caused by plain C# objects choose one of the following options:

### First
:::note
#### Assign a value (choose one)  
- [Serialize the field](../../../Serialization/Serializing%20A%20Field%201.md) where appropriate.
- Assign the reference using [`new`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator). This can be done inline or in a method like [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) or [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html).  

:::  
### Then
:::note
#### Check the value isn't `null` before you access it (choose one)  
- Exit early if `null`:
    ```csharp
    if (example == null)
    {
        // Exit early.
        return;
    }
    // Code that uses example.
    ```
- Nest your code in a `null` check:
    ```csharp
    if (example != null)
    {
        // Code that uses example.
    }
    ```
- Use a [null conditional operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-):
    ```csharp
    example?.Do(); // Example of null-conditional member access.
    ```

:::  
**Or:**  
:::note
#### Ensure nothing assigns the reference to `null` before you access it.
:::