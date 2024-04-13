# InvalidOperationException: Collection was modified
## List removal
### Reverse for loop
Perform a reverse for loop, and remove by index:

```csharp
for (int i = list.Count - 1; i >= 0; i--)
{
    if (condition)
        list.RemoveAt(i);
}
```

Starting removals at the end of the list improves performance, and you don't need to modify the counter after a removal.

:::info{.small}
You can autocomplete `for` to get a correctly written for loop, and `forr` for a reverse loop.
:::

### Linq RemoveAll
If your loop contains a simple condition, you can use [`RemoveAll`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.removeall) to remove the matching elements.
```csharp
// Remove all list elements that match a condition.
list.RemoveAll(element => condition);
```

## Removal without maintaining order
If you want to improve the performance of removals even further and don't care about the index of elements in your list you can use an unordered removal technique.

This extension method is called `RemoveUnorderedAt`, but you may also see it called `RemoveAtSwapBack`, which describes how it operates. Because the last value is "swapped back" into the index we're removing, you need to ensure it's been considered when looping.  
Again, a reverse for loop helps here to avoid manually manipulating indices.

```csharp
for (int i = list.Count - 1; i >= 0; i--)
{
    if (condition)
        list.RemoveUnorderedAt(i);
}
```

### `CollectionExtensions.cs`

```csharp
using System.Collections.Generic;

public static class CollectionExtensions
{
    /// <summary>
    /// Removes an item from a list without caring about maintaining order.<br/>
    /// (Moves the last element into the hole and removes it from the end)
    /// </summary>
    public static void RemoveUnorderedAt<T>(this IList<T> list, int index)
    {
        int endIndex = list.Count - 1;
        list[index] = list[endIndex];
        list.RemoveAt(endIndex);
    }
    
    /* Collapsable: RemoveUnordered<T> */
    /// <summary>
    /// Removes an item from a list without caring about maintaining order.<br/>
    /// (Moves the last element into the hole and removes it from the end)
    /// </summary>
    public static void RemoveUnordered<T>(this IList<T> list, T item)
    {
        int indexOf = list.IndexOf(item);
        if (indexOf < 0)
            return;
        list.RemoveUnorderedAt(indexOf);
    }
    /* End Collapsable */
}
```

---

[I cannot use a `for` loop here.](Collection%20Removal.md)
