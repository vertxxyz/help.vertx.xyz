# InvalidOperationException: Collection was modified
## Setting values in collections

There are a couple of options for modifying collections when iterating.

::::note
### Create a temporary copy of the collection before iterating over the copy.
```csharp
var copy = new CollectionType(collection);
foreach (var item in copy)
{
    if (condition)
        collection[key] = modification;
}
```
:::info{.small}  
`CollectionType` should be replaced with the type used to create the original collection.
:::  
::::

**Or:**

::::note
### Create a temporary collection used to set values after the loop.
```csharp
var modifications = new List<(KeyType key, ItemType value)>();
foreach (var item in collection)
{
    if (condition)
    {
        // Cache a deferred modification.
        modifications.Add((key, modification));
    }
}

// Apply modifications to the collection.
foreach (var pair in modifications)
{
    collection[pair.key] = pair.value;
}
```
:::info{.small}  
`KeyType` should be replaced with the type used for values in your collection.  
`ItemType` should be replaced with the type used to key modifications.  
:::  
::::

:::info  
To reduce the garbage collection impact of either of these methods you can use a pooled collection, like one retrieved from the built-in collection pools like [`ListPool`](https://docs.unity3d.com/ScriptReference/Pool.ListPool_1.html), [`HashSetPool`](https://docs.unity3d.com/ScriptReference/Pool.HashSetPool_1.html), or [`DictionaryPool`](https://docs.unity3d.com/ScriptReference/Pool.DictionaryPool_2.html).  
:::
