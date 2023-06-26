## InvalidOperationException: Collection was modified

When iterating over a collection using a foreach loop you are using an iterator that maintains the state of that loop.  
Modifying the collection while looping like this will cause the iterator to become invalid, so an exception is thrown.

```csharp
foreach (var item in collection)
{
    if (condition)
        collection.Remove(item); // InvalidOperationException.
}
```

### Resolution
#### I am removing some items from a list
Instead, perform a reverse for loop (you can autocomplete `forr` to get one), and remove by index:

```csharp
for (int i = collection.Count - 1; i >= 0; i--)
{
    if (condition)
        collection.RemoveAt(i);
}
```

Starting removals at the end of the list improves performance, and you don't need to modify the counter after a removal.

#### I am adding items to a list
Instead, use a for loop (forward or reverse). You must correctly manage the counter to avoid infinite loops.
```csharp
for (int i = collection.Count - 1; i >= 0; i--)
{
    if (condition)
        collection.Add(newItem);
}
```

#### I am modifying another collection
Instead, you will need either need to:  
:::note  
Create a temporary copy of the collection before iterating over the copy.**
```csharp
var copy = new CollectionType(collection);
foreach (var item in copy)
{
    if (condition)
        collection.Remove(item);
}
```
:::info{.inline}  
`CollectionType` should be replaced with the type used to create the original collection.
:::  
:::  
**Or:**  
:::note  
Create a temporary collection used for removals after the loop.
```csharp
var toRemove = new List<ItemType>();
foreach (var item in collection)
{
    if (condition)
        toRemove.Add(item);
}

foreach (var item in toRemove)
{
    collection.Remove(item);
}

```
:::info{.inline}  
`ItemType` should be replaced with the type used to key removals.
:::  
:::  

:::info  
To reduce the garbage collection impact of either of these methods you can use a pooled collection, like one retrieved from the built-in collection pools like [`ListPool`](https://docs.unity3d.com/ScriptReference/Pool.ListPool_1.html), [`HashSetPool`](https://docs.unity3d.com/ScriptReference/Pool.HashSetPool_1.html), or [`DictionaryPool`](https://docs.unity3d.com/ScriptReference/Pool.DictionaryPool_2.html).
:::