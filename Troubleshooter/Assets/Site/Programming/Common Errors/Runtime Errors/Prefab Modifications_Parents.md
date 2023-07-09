## Prefab modifications: Parents
```
Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption.
```

Prefab assets are GameObjects in your Project. You do not want to modify them, this would cause permanent modifications to your project.

### Resolution
#### I do not want to modify a prefab asset
If you are seeing this error, you have probably referenced a prefab, and are trying to modify it instead of the instance.
If the variable you are calling methods on is the same as your prefab, your setup is wrong.

::::note  
[`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) has an overload that takes a parent, use that instead.

```csharp
[SerializeField] private PrefabComponentType _prefab;
private PrefabComponentType _instance;

void SpawnAndParent(Transform parent)
{
    // Spawn our prefab, set its parent, and reference the new instance.
    _instance = Instantiate(_prefab, parent);
}
```

:::  
::::note  
[`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) returns the instance it spawned. Store a reference to it and modify that instead.
```csharp
[SerializeField] private PrefabComponentType _prefab;
private PrefabComponentType _instance;

void SpawnAndParent(Transform parent)
{
    // Spawn our prefab and reference the new instance.
    _instance = Instantiate(_prefab);
    // Set the parent of our prefab
    _instance.SetParent(parent);
}
```

:::  

If you're confused by the above code, see [referencing prefabs from scenes](../../References/References%20To%20Prefabs.md).

#### I know what I'm doing, and want to modify a prefab asset

// TODO