## NullReferenceException: Plain C# objects
To resolve a `NullReferenceException` caused by a `null` `object` that isn't a `UnityEngine.Object` type you can choose one of the following options:

**Either:**  
:::note
#### Assign a value (choose one)  
- [Serialize the field](../../../Variables/Serialization/Serializing%20A%20Field%201.md) if appropriate.
- Assign the reference using [`new`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator). This can be done inline or in a method like [Awake](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) or [Start](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html).  
:::  
**Or:**  
:::note
#### Check the value isn't `null` before you access it (choose one)  
- Exit early if `null`.
    ```csharp
    if (example == null)
    {
        // Exit early.
        return;
    }
    // Code that uses example.
    ```
- Nest your code in a `null` check.
    ```csharp
    if (example != null)
    {
        // Code that uses example.
    }
    ```
:::  
**Or:**  
:::note
#### Ensure nothing assigns `null` before you access it.
:::