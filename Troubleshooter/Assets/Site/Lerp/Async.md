## Lerp: Async await

You can create async functions that move between two values over a period of time by incrementing a timer, and normalising the value to `0->1` by dividing by the duration.

:::warning  
This requires [`Awaitable`](https://docs.unity3d.com/2023.1/Documentation/ScriptReference/Awaitable.html) which was introduced in 2023.1.
:::  

### Example

```csharp
async void MoveTo(Vector3 destination, float duration = 1)
{
    Vector3 origin = transform.position;
    float elapsedTime = 0;
    while(elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(origin, destination, elapsedTime / duration);
        await Awaitable.NextFrameAsync();
    }
    transform.position = destination;
}
```

Remember to use [`CancellationTokens`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) if you're calling this function while it's already running.  
Similar logic can be applied to all lerp variations, and is commonly used in tweening libraries.

The result is a linear movement from one value to another. If you want to add **easing**, either [`Evaluate`](https://docs.unity3d.com/ScriptReference/AnimationCurve.Evaluate.html) your `t` value with an [`AnimationCurve`](https://docs.unity3d.com/ScriptReference/AnimationCurve.html) (you can configure one via the inspector), or pass it through an [easing function](https://easings.net).

---  
[Return to overview.](Overview.md)