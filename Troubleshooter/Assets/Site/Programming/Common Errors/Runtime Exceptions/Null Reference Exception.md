## Null reference exception
### Description
A `NullReferenceException` (**NRE**), occurs when code tries to access a variable which isn’t set, or found.  
:::info
Declaring a reference variable does not automatically assign it an appropriate value.
:::

### Troubleshooting:
Check accessed variables on the line throwing the exception[^1] for `null`.  

Only reference types can be `null`. Ie. `class`, `interface`, or `delegate` variables.  
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
1. **Either:**  
    Assign a reference to the variable (choose one)
    - Drag and drop the reference in the component's inspector[^2]. (If your field is serialized by Unity)  
    - Manually assign the reference in a method like `Awake` or `Start` using [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) or similar.
    - Initialise the variable with [AddComponent](https://docs.unity3d.com/ScriptReference/GameObject.AddComponent.html) or [CreateInstance](https://docs.unity3d.com/ScriptReference/ScriptableObject.CreateInstance.html).

    **Or:**  
    Check whether the reference is not null before trying to access it  
    `if(example != null)`  
    If looking to combine with GetComponent, use [TryGetComponent](https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html) instead.
3. Ensure that nothing is destroying the Object, or setting it to `null` before you attempt to use it.
4. Check prior methods make correct assumptions. For example, `GetComponent<Example>()` will return `null` if an `Example` component is not attached to the same GameObject.  
:::
#### Other classes
:::note
1. **Either:**  
   Assign a reference to the variable (choose one)
    - [Serialize the field](../../Variables/Serialization/Serialization%201/Serializing%20A%20Field%201.md) if appropriate.
    - Assign the reference using `new`. This can be done inline or in a method like `Awake` or `Start`.

   **Or:**  
   Check whether the reference is not null before trying to access it  
   `if(example != null)`
2. Ensure that nothing is setting it to `null` before you attempt to use it.
:::

---  

See [Unity Null](../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.

[^1]: See [Error Stack Traces](../Stack%20Traces.md) to learn how to find line numbers for exceptions.  
[^2]: See [Serializing Component References](../../Variables/Other%20Members/Serializing%20Component%20References.md) to learn how to assign variables in the Inspector.  