# Physics queries (2D): NonAlloc

If you are using non-allocating versions of `Physics2D` queries, do not read results that are outside of the returned range as they are invalid.

```csharp    
// 🔴 Incorrect, the return value is never read.
Physics2D.CircleCast(origin, radius, direction, filter, results);
foreach (RaycastHit2D hit in results)
{
    // ...    
}

// 🟢 Correct, the return value is used in a for loop.
int hitCount = Physics2D.CircleCast(origin, radius, direction, filter, results);
for (int i = 0; i < hitCount; i#)
{
    RaycastHit2D hit = results[i];
    // ...    
}
```

---

[I am still having problems with my query.](Visual%20Debugging%202D.md)
