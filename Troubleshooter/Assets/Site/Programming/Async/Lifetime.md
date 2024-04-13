# Lifetime of async functions
Unlike coroutines, async functions are not tied to the lifetime of a UnityEngine object.
This not only means that they will continue when an object is destroyed, but also across Play Mode boundaries and into Edit mode.

## Resolution
Use a [`CancellationToken`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) to exit out of the async function early.  
Unity provides [`destroyCancellationToken`](https://docs.unity3d.com/ScriptReference/MonoBehaviour-destroyCancellationToken.html), which is raised when a MonoBehaviour is destroyed. This gives you similar behaviour to a Coroutine's lifetime.

### Example
```csharp
public async void WaitAndPerformAction(float delay, Action callback)
{
    try   
    {
        // Pass our arguments and destroyCancellationToken to the async function.
        await WaitAndPerformActionAsync(delay, callback, destroyCancellationToken);
    }
    // We don't need to actually do anything when the OperationCanceledException is raised.
    catch(OperationCanceledException) { }
}

/// <summary>
/// Waits for <see cref="delay"/> seconds and then invokes <see cref="callback"/>.
/// </summary>
/// <exception cref="OperationCanceledException">Raised if <see cref="cancellationToken"/> is cancelled.</exception>
private async Awaitable WaitAndPerformActionAsync(float delay, Action callback, CancellationToken cancellationToken)
{
    // WaitForSecondsAsync will throw an OperationCanceledException when cancellationToken is raised.
    await Awaitable.WaitForSecondsAsync(delay, cancellationToken);
    callback();
}
```

## Details
If you wanted to exit your function if cancellationToken was raised, you can call `cancellationToken.ThrowIfCancellationRequested();`.  
If you just want to check whether it has been raised, you can check `cancellationToken.IsCancellationRequested`.  

Using `IsCancellationRequested` and exiting a function will be light-weight in comparison to raising exceptions, but that will come at the cost of detailed handling of the cancellation all the way up the stack.
Decide which method works best for you, and add a summary XML tag for your function if it can raise an exception so programmers that call it are more aware of the implementation you chose.
