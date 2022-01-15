## Null reference exception
### Description
A `NullReferenceException` (NRE), occurs when code tries to access a variable which isn’t set, or found.  

```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction: right
#.x: visual=none stroke=#f55 body=bold

[<reference>null]
[<x>?]
[variable]->[null]
[null]-->[?]
```

:::warning
Declaring a reference variable does not automatically assign it a value.
:::

### Troubleshooting:
Check accessed variables on the [line throwing the exception](../Stack%20Traces.md) for `null`.  

Only [reference types](../../Value%20And%20Reference%20Types.md) can be `null`. Ie. `class`, `interface`, or `delegate` variables.  
Access is denoted by:
- `.`  - member access.
- `[]` - array element or indexer access.
- `()` - invocation.

:::info { .inline }
See [General Debugging](../../Debugging.md) for troubleshooting techniques.
:::

### Resolution
#### UnityEngine.Object types
:::note
- **Either:**  
    Assign a reference to the variable (choose one)
    - Drag and drop the reference in the component's inspector[^1] (if your field is serialized by Unity).  
    - Manually assign the reference in a method like `Awake` or `Start` using [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) or similar.
    - Initialise the variable with [AddComponent](https://docs.unity3d.com/ScriptReference/GameObject.AddComponent.html) or [CreateInstance](https://docs.unity3d.com/ScriptReference/ScriptableObject.CreateInstance.html).

    **Or:**  
    Check the reference is not null before trying to access it  
    `if (example != null)`  
    If looking to combine with GetComponent, use [TryGetComponent](https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html) instead.
- Ensure that nothing is destroying the Object, or setting it to `null` before you attempt to use it.
- Check assignments use correct assumptions. Some examples: 
   - `GetComponent<Example>()` will return `null` if an `Example` component is not attached to the same GameObject.  
   - `Camera.main` will return null if there is not a camera tagged as MainCamera.  
- Check that you are not using modern null checking operators[^2] (`?.`, `??`, `??=`).
:::
#### Other classes
:::note
- **Either:**  
   Assign a reference to the variable (choose one)
    - [Serialize the field](../../Variables/Serialization/Serialization%201/Serializing%20A%20Field%201.md) if appropriate.
    - Assign the reference using [`new`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator). This can be done inline or in a method like `Awake` or `Start`.

   **Or:**  
   Check the reference is not null before trying to access it  
   `if (example != null)`
- Ensure that nothing sets it to `null` before you attempt to use it.
:::

[^1]: See [Serializing Component references](../../Variables/Other%20Members/Serializing%20Component%20References.md) to learn how to assign variables in the Inspector.  
[^2]: See [Unity null](../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.