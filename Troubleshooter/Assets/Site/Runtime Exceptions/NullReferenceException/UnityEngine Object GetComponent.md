## NullReferenceException: UnityEngine.Object — GetComponent
Check your usage of `GetComponent` or its variants:
<details>
<summary>

#### [`GetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) or [`TryGetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html)
</summary>

A component of the target type must be attached to the same GameObject the function is called on. If the value is `null` after calling, then this assumption is wrong.
1. The **same** GameObject has a component matching the argument.
1. The target components' type actually matches[^1].
1. [Search the scene](../../Scene%20View/Searching.md) for all instances of the caller and check all results (`t:ExampleComponent` for example).
1. Assignment must occur before access. Often you would use [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) to get, and [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html) to use.

:::info{.small}
You can use the [`RequireComponent`](https://docs.unity3d.com/ScriptReference/RequireComponent.html) attribute to automatically add component dependencies when adding new components in the editor.
:::
</details>

<details>
<summary>

#### [`GetComponentInChildren`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponentInChildren.html)
</summary>

Only components on active GameObjects are returned unless the `includeInactive` argument is `true`.
1. Either the same object or a child has a matching component.
1. The component type actually matches[^1].
1. [Search the scene](../../Scene%20View/Searching.md) for all instances of the caller and check all results (`t:ExampleComponent` for example).
1. If the component is on an inactive child, provide the `includeInactive` argument as `true`.
1. Assignment must occur before access. Often you would use [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) to get, and [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html) to use.

</details>

<details>
<summary>

#### [`GetComponentInParent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponentInParent.html)
</summary>

Only components on active GameObjects are returned unless the `includeInactive` argument is `true`.
1. Either the same object or a parent has a matching component.
1. The component type actually matches[^1].
1. [Search the scene](../../Scene%20View/Searching.md) for all instances of the caller and check all results (`t:ExampleComponent` for example).
1. If the component is on an inactive parent, provide the `includeInactive` argument as `true`.
1. Assignment must occur before access. Often you would use [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) to get, and [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html) to use.

</details>


---

If you're still having issues using these methods, it's often preferable to [serialize components via the inspector](../../References/Serializing%20Component%20References.md) instead.

:::warning
Double-check the assignment is actually executed.  
Your code may not be called because of an incorrect setup like a misspelt method name.
:::

[^1]: A common example of this being wrong is `Text` when using subtypes of `TMP_Text`.
