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

#### I am using a `List`:
- [I am **removing** items.](Collection%20Was%20Modified/List%20Removal.md)
- [I am **adding** items.](Collection%20Was%20Modified/List%20Addition.md)

#### I am using another collection type:
- [I am **removing** items.](Collection%20Was%20Modified/Collection%20Removal.md)
- [I am **adding** items.](Collection%20Was%20Modified/Collection%20Addition.md)