# Physics queries (3D): NonAlloc

If you are using non-allocating versions of `Physics` queries, don't read results that are outside of the returned range as they are invalid.

```csharp    
// 🔴 Incorrect, the return value is never read.
Physics.SphereCastNonAlloc(ray, radius, results);
foreach (RaycastHit hit in results)
{
    // ...    
}

// 🟢 Correct, the return value is used in a for loop.
int hitCount = Physics.SphereCastNonAlloc(ray, radius, results);
for (int i = 0; i < hitCount; i++)
{
    RaycastHit hit = results[i];
    // ...    
}
```

---

[I am still having problems with my query.](Visual%20Debugging%203D.md)
