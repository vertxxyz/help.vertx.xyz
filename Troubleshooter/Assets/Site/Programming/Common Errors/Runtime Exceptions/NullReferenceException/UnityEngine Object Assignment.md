## NullReferenceException: UnityEngine.Object
To resolve a `NullReferenceException` caused by a `null` `UnityEngine.Object` you can choose one of the following options:  

- ::collapse::{.collapse}
::::note
**Either:**  
:::note
  #### Assign a reference to the variable (choose one)
  - Drag and drop the reference in the component's inspector[^1] (if your field is serialized by Unity).
  - Manually assign the reference in a method like `Awake` or `Start` using [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) or similar.
  - Initialise the variable with [AddComponent](https://docs.unity3d.com/ScriptReference/GameObject.AddComponent.html) or [CreateInstance](https://docs.unity3d.com/ScriptReference/ScriptableObject.CreateInstance.html).
:::  
  **Or:**  
:::note
  #### Check the reference isn't null before trying to access it  
  `if (example != null)`  
  If looking to combine with GetComponent, use [TryGetComponent](https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html) instead.
:::
::::
- Ensure that nothing is destroying the Object, or setting it to `null` before you attempt to use it.
- Check assignments use correct assumptions. Some examples:
    - `GetComponent<Example>()` will return `null` if an `Example` component isn't attached to the same GameObject.
    - `Camera.main` will return null if there isn't a camera tagged as MainCamera.
- Check that you are not using modern null checking operators[^2] (`?.`, `??`, `??=`).

---

If the object is assigned in the Inspector, search the Scene (eg. `t:ExampleComponent`), ensuring there aren't duplicate components causing the issue.  
Logs can also be made to ping objects they reference using the [context parameter](../../../Debugging/Logging/Logging%20How-to.md).

[^1]: See [Serializing Component references](../../../Variables/Other%20Members/Serializing%20Component%20References.md) to learn how to assign variables in the Inspector.  
[^2]: See [Unity null](../../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.  