## InvalidOperationException: Collection was modified
### Collection removal

There are a couple of options for modifying collections when iterating.

::::note
#### Create a temporary copy of the collection before iterating over the copy.
```csharp
var copy = new CollectionType(collection);
foreach (var item in copy)
{
    if (condition)
        collection.Remove(item);
}
```
:::info{.small}  
`CollectionType` should be replaced with the type used to create the original collection.
:::  
::::

**Or:**

::::note  
#### Create a temporary collection used for removals after the loop.
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
:::info{.small}  
`ItemType` should be replaced with the type used to key removals.
:::  
::::

:::info  
To reduce the garbage collection impact of either of these methods you can use a pooled collection, like one retrieved from the built-in collection pools like [`ListPool`](https://docs.unity3d.com/ScriptReference/Pool.ListPool_1.html), [`HashSetPool`](https://docs.unity3d.com/ScriptReference/Pool.HashSetPool_1.html), or [`DictionaryPool`](https://docs.unity3d.com/ScriptReference/Pool.DictionaryPool_2.html).  
:::