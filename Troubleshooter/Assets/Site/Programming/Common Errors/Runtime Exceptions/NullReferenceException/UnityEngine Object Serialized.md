## NullReferenceException: UnityEngine.Object — Serialized
**Either:**
:::note
#### Find the cause of `null` values

1. Check that all references have been assigned in the inspector[^1].
1. Search the Scene (`t:ExampleComponent` for example) when the error occurs, ensuring there aren't duplicate components causing the issue.
1. Remove any `GetComponent` calls that could be overriding your reference[^2].
1. Check that you are not using modern null checking operators[^3] (`?.`, `??`, `??=`).
1. Ensure nothing is destroying the Object, or setting it to `null` before you attempt to use it.

Logs can also be made to ping objects they reference using the [context parameter](../../../Debugging/Logging/Logging%20How-to.md).  
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
Do not use modern null checking operators[^3] (`?.`, `??`, `??=`) to check for null.  
:::

[^1]: See [Serializing Component references](../../../Variables/Other%20Members/Serializing%20Component%20References.md) to learn how to assign variables in the Inspector.  
[^2]: A common mistake is missing `GetComponent` setting a serialized value to `null` in `Awake` or `Start`.  
[^3]: See [Unity null](../../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.  