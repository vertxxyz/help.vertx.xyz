# Destroying assets
```
Destroying assets is not permitted to avoid data loss.
```

Assets are imported objects in your Project. You do not want to destroy them, as that will delete the original, causing permanent damage to your project.

## Resolution
### I do not want to destroy an asset
If you are seeing this error, you have probably referenced a prefab, and are trying to destroy it instead of the instance.  
[`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) returns the instance it spawned. Store a reference to it and destroy that instance.  
If the variable you are calling `Destroy` on is the same as your prefab, your setup is wrong.

```csharp
[SerializeField]
private MyScript _prefab;
private MyScript _instance;

void SpawnAndDestroyLater(float delay)
{
    // Spawn our prefab and reference the new instance.
    _instance = Instantiate(_prefab);
    // Destroy the new object after a delay.
    Destroy(_instance, delay);
}
```

### I know what I'm doing, and want to destroy an asset
Use `DestroyImmediate`, and pass `true` as the final parameter.  
:::warning{.small}  
This can damage your project when done incorrectly.  
:::

## Notes
I like to add the suffixes `Prefab`/`Template`/`Asset` and `Instance` to make it clear what is being worked with.  
Use consistent conventions (like the [C# naming conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names#naming-conventions)) to reduce mistakes.
