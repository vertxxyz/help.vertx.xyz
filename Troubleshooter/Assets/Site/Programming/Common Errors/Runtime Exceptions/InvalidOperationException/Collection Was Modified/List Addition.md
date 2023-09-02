## InvalidOperationException: Collection was modified
### List addition
Use a reverse for loop. Because the loop is done in reverse, the elements added to the end are not iterated when you add them, because the index approaches 0. 
```csharp
for (int i = list.Count - 1; i >= 0; i--)
{
    if (condition)
        list.Add(newItem);
}
```

You can perform the above logic with a forward loop, but you must correctly manage the counter to avoid infinite loops. Caching the collection count instead of evaluating it in the `for` would prevent it from iterating over the newly added elements.

:::info{.small}
You can autocomplete `for` to get a correctly written for loop, and `forr` for a reverse loop.
:::

---

[I cannot use a `for` loop here.](Collection%20Addition.md)