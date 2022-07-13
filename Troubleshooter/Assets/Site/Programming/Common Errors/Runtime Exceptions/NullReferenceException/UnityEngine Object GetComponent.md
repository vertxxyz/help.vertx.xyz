## NullReferenceException: UnityEngine.Object — GetComponent
Check your usage of `GetComponent` or its variants:  
### [`GetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) or [`TryGetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html)
A component of the target type must be attached to the same gameobject the caller is. If the value is `null` after calling, then this assumption is wrong.
1. The **same** gameobject has a component matching the argument.
1. The target components' type actually matches[^1].  
1. Search the scene for all instances of the caller and check the assumption is true (`t:ExampleComponent` for example).

### [`GetComponentInChildren`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponentInChildren.html)
Only components on active gameobject are returned unless the `includeInactive` argument is `true`.
1. Either the same object or a child has a matching component.
1. The component type actually matches[^1].  
1. Search the scene for all instances of the caller and check the assumption is true (`t:ExampleComponent` for example).
1. If the component is on an inactive child, provide the `includeInactive` argument as `true`.

### [`GetComponentInParent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponentInParent.html)
Only components on active gameobject are returned unless the `includeInactive` argument is `true`.
1. Either the same object or a parent has a matching component.
1. The component type actually matches[^1].  
1. Search the scene for all instances of the caller and check the assumption is true (`t:ExampleComponent` for example).
1. If the component is on an inactive parent, provide the `includeInactive` argument as `true`.

[^1]: A common example of this being wrong is `Text` when using subtypes of `TMP_Text`.