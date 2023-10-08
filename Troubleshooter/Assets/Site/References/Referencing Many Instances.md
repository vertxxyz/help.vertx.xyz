## Referencing many instances
To reference many instances, you must create a collection to hold them in. It's useful to understand:
- [Arrays](https://learn.unity.com/tutorial/arrays-9o); a fixed-size collection.
- [Lists](https://learn.unity.com/tutorial/lists-and-dictionaries-1); a resizable collection.
- [Dictionaries](https://learn.unity.com/tutorial/lists-and-dictionaries-1); a collection that associates each value to a unique key.

### Serialized references
You can only serialize Arrays and Lists. Dictionaries are not serializable without extra work.  

::::
#### 1. Expose a serialized reference to a collection containing the target component
The [field](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields) must be marked with [`SerializeField`](https://docs.unity3d.com/ScriptReference/SerializeField.html):
```csharp
[SerializeField] private Transform[] _targets;
```
**or** can be `public`:
```csharp
public Transform[] Targets;
```

:::info{.small}
This example uses the `Transform` type, it will need to be replaced with the target type.
:::  

#### Choosing to serialize Arrays or Lists
As serialized references are set in the inspector, it's a good to *only* set them there to avoid confusion. With this in mind, serializing Arrays instead of Lists reduces the ways code can modify the data.

Some people suggest *only* using Lists to avoid thinking about which is which.

The practical differences are few, and a good IDE will make it easy to work with both, so make a choice and set some guidelines with your team to be consistent.

::::  
::::note  
#### 2. Reference the target component in the Inspector
Do not directly reference the script asset. The target component must be an instance [added to an object in the scene](https://docs.unity3d.com/Manual/UsingComponents.html).

^^^
<video width="750" height="200" autoplay loop muted controls><source type="video/webm" src="https://unity.huh.how/Video/inspector-references-array.webm"></video>
^^^ ::Empty slots **Component** will result in an [`UnassignedReferenceException`](../Runtime%20Exceptions/UnassignedReferenceException.md).::{.error}

Dragging a GameObject from the [Hierarchy](https://docs.unity3d.com/Manual/Hierarchy.html) into the field will reference the first matching Component found on the object.

You can drag multiple instances into the array at a time by either locking the inspector, or by opening a temporary inspector via the **Properties...** menu in the Component's context menu.  
::::  
::::note  
#### 3. Access the member you care about
`public` members on the individual instances can be accessed via a loop. Or by directly indexing into the collection.
```csharp
foreach (var target in _targets)
{
    // Variables and properties
    var variable = target.Variable;
    target.Variable = variable;
    
    // Methods
    target.Method();
}
```
:::info{.small}
If you don't have autocomplete, [configure your IDE](../IDE%20Configuration.md) to easily find member names and get error highlighting.
:::

The usage must be at a [method or block level scope](../Programming/Other/Scopes.md).  
::::  

### Referencing instantiated objects

#### Using a List
Use a list when you have a number of instances to spawn, and don't need to look up individual instances.

^^^
```csharp
[SerializeField] private Transform _prefab;
// An initialised list to associate spawned players with indices.
private readonly List<Transform> _targets = new();

/// <summary>
/// Gets the spawned targets.
/// </summary>
public IReadOnlyList<Transform> Targets => _targets;

/// <summary>
/// Spawns a new target at <see cref="position"/>.
/// </summary>
public void SpawnNewTarget(Vector3 position)
{
    Transform instance = Instantiate(_prefab);
    instance.localPosition = position;
    _targets.Add(instance);
}
```
^^^ Don't forget to initialise the List or you will get a [NullReferenceException](../Runtime%20Exceptions/NullReferenceException.md).

#### Using a Dictionary
Use a dictionary when your instances must be associated with a key. Dictionaries provide a fast lookup, avoiding loops when trying to find data.

^^^
```csharp
[SerializeField] private Player _playerPrefab;
// An initialised dictionary to associate spawned players with indices.
private readonly Dictionary<int, Player> _players = new();

/// <summary>
/// Spawns a player and associates it with an index.
/// </summary>
/// <param name="index">The index to associate with the spawned player.</param>
/// <returns>The newly spawned player.</returns>
/// <exception cref="ArgumentException">Exception thrown if there already a player associated with the index.<br/>Use <see cref="GetRequiredPlayer"/> if you are unsure whether it was spawned.</exception>
public Player SpawnPlayer(int index)
{
    // Check if the player already exists
    if (_players.ContainsKey(index))
        throw new ArgumentException($"Player at index {index} has already been spawned.");
    
    // Spawn our player.
    Player playerInstance = Instantiate(_playerPrefab);
    // Cache our new player instance so TryGetPlayer and GetRequiredPlayer will return it.
    _players.Add(index, playerInstance);
    // Initialise the player with the index, it's helpful to find that information on the player too.
    playerInstance.Initialise(index);
    
    // Return the newly spawned player.
    return playerInstance;
}

/// <summary>
/// Gets a player if they were already spawned.
/// </summary>
/// <param name="index">The index associated with the player</param>
/// <param name="player">The returned player that was associated with the index.</param>
/// <returns>True if the player was found.</returns>
public bool TryGetPlayer(int index, out Player player)
    => _players.TryGetValue(index, out player);

/// <summary>
/// Gets a player, spawning it if required.
/// </summary>
/// <param name="index">The index associated with the player.</param>
/// <returns>The player associated with the index, found or spawned.</returns>
public Player GetRequiredPlayer(int index)
    // If the player already exists, return it, otherwise spawn a new instance.
    => TryGetPlayer(index, out Player player) ? player : SpawnPlayer(index);
```
^^^ Don't forget to initialise the Dictionary or you will get a [NullReferenceException](../Runtime%20Exceptions/NullReferenceException.md).

If you have a fixed, small number of players you may consider using an array, just checking whether the index is occupied (not `null`).
