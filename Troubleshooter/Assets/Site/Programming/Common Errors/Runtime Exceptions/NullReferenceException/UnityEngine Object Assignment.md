## NullReferenceException: UnityEngine.Object
If you want to handle the case where the object is unassigned:  
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

If you want to correctly assign the object:  
:::note
#### Find the cause of `null` values
- [I am serializing a reference in the inspector.](UnityEngine%20Object%20Serialized.md)
- [I am using `GetComponent` or its variants](UnityEngine%20Object%20GetComponent.md)
- [I am using `Find` or its variants](UnityEngine%20Object%20Find.md)
- [I am using `AddComponent` or `CreateInstance`](UnityEngine%20Object%20Add%20Or%20Create.md)
- [I am using `Camera.main`.](UnityEngine%20Object%20Camera%20Main.md)
- [I am using another method.](UnityEngine%20Object%20General.md)
- [I am not sure.](UnityEngine%20Object%20General.md)

:::

[^1]: See [Unity null](../../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.  