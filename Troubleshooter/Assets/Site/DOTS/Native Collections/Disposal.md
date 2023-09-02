## Native collections: Disposal issues
```
ObjectDisposedException: The collection has been deallocated, it is not allowed to access it
```

```
InvalidOperationException: The X YJob.Value has been deallocated. All containers must be valid when scheduling a job.
```

### Resolution

1. Check that you have allocated the native collection using a non-default constructor, the constructor must take an [`Allocator`](https://docs.unity3d.com/ScriptReference/Unity.Collections.Allocator.html).
1. Check that you have not prematurely called `Dispose` on your native collection.
1. Check that a job hasn't prematurely disposed the collection via `[DeallocateOnJobCompletion]`.
1. If you are using `Allocator.Temp` please check the [known issues](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/manual/issues.html) page to ensure you haven't invalidated a safety handle.
1. Check that you haven't disposed the collection in a previous iteration of a loop.