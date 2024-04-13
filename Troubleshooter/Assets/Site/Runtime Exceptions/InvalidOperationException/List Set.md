# InvalidOperationException: Collection was modified
## Setting list values while using a foreach
Use a `for` loop instead. You should avoid setting values while iterating with a `foreach`.
```csharp
for (int i = 0; i < list.Count; i#)
{
    if (condition)
        list[i] = new();
}
```

---

[I cannot use a `for` loop here.](Collection%20Set.md)
