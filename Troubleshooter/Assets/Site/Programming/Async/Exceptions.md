## Exceptions in async functions
The [`Task`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) and [`Awaitable`](https://docs.unity3d.com/2023.1/Documentation/ScriptReference/Awaitable.html) structures capture exceptions which are later raised when they are awaited. This is partially because an async function can collect many exceptions under certain conditions (using an [`AggregateException`](https://learn.microsoft.com/en-us/dotnet/api/system.aggregateexception)).

### Resolution
When you create a `Task` or `Awaitable` by calling an async function that returns one, you **must** `await` that call.  
In Unity, exceptions will be caught by the engine if they are raised at a top-level function, so you don't need to catch in an async `void`-returning function.

In practice this means that top-level "fire-and-forget" functions that do not need to be waited for are `async void`, and all functions that are waited on are `async Task` or `async Awaitable` (or their generic versions).

### Implementation
Entry fire-and-forget functions are `async void` and are not awaited. Any `async Awaitable` or `async Task` that is called uses `await`.

#### Example
```csharp
// We don't (and can't) await this function, it is fire-and-forget
public async void WaitAndPerformAction(float delay, Action callback)
{
    try   
    {
        // Make sure to await this, as it returns an Awaitable.
        // If the Awaitable contains an exception it will be thrown when awaited.
        await WaitAndPerformActionAsync(delay, callback, destroyCancellationToken);
    }
    catch(OperationCanceledException) { }
}

// Never call this async Awaitable without awaiting it.
private async Awaitable WaitAndPerformActionAsync(float delay, Action callback, CancellationToken cancellationToken)
{
    // Awaitable's methods also return an Awaitable, and must be awaited (of course it also won't wait if not awaited)!
    await Awaitable.WaitForSecondsAsync(delay, cancellationToken);
    callback();
}

```

---

See [lifetime of async functions](Lifetime.md) if you want to learn more about the cancellation token setup in the above example.
