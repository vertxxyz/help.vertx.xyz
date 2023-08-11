## GetComponent methods
[`GetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html), [`TryGetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html),
[`GetComponentInChildren`](https://docs.unity3d.com/ScriptReference/Component.GetComponentInChildren.html), and other similar methods are perfect for dynamic runtime references like those gathered in a physics message or query.  

It's preferable to use [serialized references](Serialized%20References.md) where possible.

### Use with interfaces
The `GetComponent` family of functions can return components that implement interfaces, making them powerful tools to work with composition.

#### Example
```csharp
using UnityEngine;

/// <summary>
/// Damages components marked with <see cref="IDamageable"/> that enters the attached trigger.
/// </summary>
public class DamageTrigger : MonoBehaviour
{
	[SerializeField] private int _damage = 100;

	void OnTriggerEnter(Collider collider)
	{
		if (!collider.TryGetComponent(out IDamageable damageable))
		{
			// Exit early, the collider's object doesn't have an IDamageable component.
			return;
		}

		// Call our Damage method.
		damageable.Damage(_damage);
	}
}

/// <summary>
/// Marks a component as capable of receiving damage.
/// </summary>
public interface IDamageable
{
	void Damage(int amount);
}
```

### Notes
- Don't use the non-generic versions of the functions unless the type is not known at compile-time.
- There is no need to cast the result of the generic versions of the functions.
- The `InChildren` and `InParent` functions also return components on the same GameObject it was called on.
- The `InChildren` and `InParent` functions take an `includeInactive` boolean, set it to true if you want to include inactive GameObjects in the search.

### Garbage collection
#### GetComponent
Due to [Unity null](../Other/Unity%20Null.md), `GetComponent` will always allocate in the editor, whereas using `TryGetComponent` will not.
#### Collection pools
Unity provides various collection pool classes (introduced in 2021.1) that can be used in combination with the multi-object `GetComponent` functions. Do note that pooled collections can last the lifetime of your application, and care must be taken when allocating them.
```csharp
using (ListPool<IDamageable>.Get(out var damageables))
{
	collider.GetComponentsInChildren(damageables);
	foreach (IDamageable damageable in damageables)
		damageable.Damage(_damage);
}
```
See [`ListPool`](https://docs.unity3d.com/ScriptReference/Pool.ListPool_1.html).