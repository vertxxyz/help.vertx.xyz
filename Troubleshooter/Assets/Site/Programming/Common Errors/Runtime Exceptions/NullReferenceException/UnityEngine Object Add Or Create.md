## NullReferenceException: UnityEngine.Object — Add or Create
**Either:**  
:::note
#### Find the cause of `null` values
1. Check that you are not using modern null checking operators[^1] (`?.`, `??`, `??=`).
1. Ensure that nothing is destroying the Object, or setting it to `null` before you attempt to use it.  
:::  
**Or:**  
:::note
#### Check the reference isn't null before trying to access it
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
Do not use modern null checking operators[^1] (`?.`, `??`, `??=`) to check for null.  
:::

[^1]: See [Unity null](../../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.  