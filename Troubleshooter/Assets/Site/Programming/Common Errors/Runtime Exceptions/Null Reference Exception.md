### Null Reference Exception
#### Description
A `NullReferenceException` or NRE, occurs when code tries to access a variable which isn’t set, or found.  
The error provides the line throwing the exception, so you can go there to narrow down what is missing.  
:::info
Declaring a reference variable does not automatically assign it an appropriate value.
:::

#### Resolution

1. **Either:**  
    Assign a reference to the variable
    - Drag and drop the reference in the component's inspector. (If your field is serialized by Unity)  
    - Manually assign the reference in a method like `Awake` or `Start` using [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html)
    - Initialise the variable with [AddComponent](https://docs.unity3d.com/ScriptReference/GameObject.AddComponent.html) or `new`.

    **Or:**  

    Check whether the reference is not null before trying to access it  
    `if(example != null)`
2. Ensure that nothing is destroying the Object, or setting it to `null` before you attempt to use it.
3. Check prior methods make correct assumptions. For example, `GetComponent<Example>()` will return `null` if an `Example` component is not attached to the same GameObject.  

#### Troubleshooting:
When troubleshooting which variable is null, be aware that only reference types can be null. Ie. `class`, `interface`, or `delegate` variables.  
Access is denoted by the `.` character (member access), `[]` (array element or indexer access), or `()` (invocation.)  
On the line throwing this exception, only variables that are being accessed via one of these methods are relevant, as only attempting to *use* a null variable will throw an NRE.  

See [General Debugging](../../Debugging.md) for troubleshooting techniques.  

---  

See [Serializing Component References](../../Variables/Other%20Members/Serializing%20Component%20References.md) to learn how to assign variables in the Inspector.  
See [Unity Null](../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.