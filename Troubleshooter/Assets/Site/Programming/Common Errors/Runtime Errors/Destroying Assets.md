## Destroying assets

Assets are imported objects in your Project. You do not want to destroy them, as that will delete the original, causing permanent damage to your project.

### Resolution
#### I do not want to destroy an asset
If you are seeing this error, you have probably referenced a prefab, and are trying to destroy it instead of the instance.  
[`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) returns the instance it spawned. Store a reference to it and destroy that instance.  
If the variable you are calling `Destroy` on is the same as your prefab, your setup is wrong.

```csharp
[SerializeField] private MyScript _prefab;
private MyScript _instance;

void SpawnAndDestroyLater(float delay)
{
    // Spawn our prefab, and collect what it created.
    _instance = Instantiate(_prefab);
    // Destroy the new object after a delay.
    Destroy(_instance, delay);
}
```

#### I know what I'm doing, and want to destroy an asset
Use `DestroyImmediate`, and pass `true` as the final parameter. This can damage your project when done incorrectly.