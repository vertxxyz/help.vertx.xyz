## References to Prefab children

You cannot directly reference children of a prefab externally.  
Instead, create a script on the root of the prefab that contains the references.

### Implementation
::::note  
#### Follow these steps to reference a child on a prefab instance:  
1. Understand how to [reference and instantiate prefabs from scenes](References%20To%20Prefabs.md) and do this for a script on the root of your prefab.  
1. [Reference the target child component](Serializing%20Component%20References.md) from the root component.
1. Now, when the prefab is [instanced](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) you can use your root component to access the child.
::::  

You can understand the root component as a manager, handling any interactions with its children. Create methods on this component to manage those objects, compartmentalising interactions instead of modifying things directly.

```nomnoml

[In-Scene Component]->[Prefab|
	[<hidden>]->[Children]
]
```

If you are still unsure what objects can reference each other, refer to the [valid references diagram](Valid%20References.md).