## Lerp: Coroutines

You can create coroutines that move between two values over a period of time by incrementing a timer, and normalising the value to `0->1` by dividing by the duration.

### Example

```csharp
public void MoveTo(Vector3 destination)
{
    StartCoroutine(DoMoveTo(destination, duration));
}

IEnumerator DoMoveTo(Vector3 destination, float duration = 1)
{
    Vector3 origin = transform.position;
    float elapsedTime = 0;
    // Wait until the time has reached the target duration.
    while (elapsedTime < duration)
    {
        // Increment our timer.
        elapsedTime += Time.deltaTime;
        // Normalise our timer, and use it to interpolate.
        transform.position = Vector3.Lerp(origin, destination, elapsedTime / duration);
        yield return null;
    }
    
    // As similar loops may not fully reach the destination, it's always worth setting it after the loop.
    transform.position = destination;
}
```

Remember to use [`StopCoroutine`](../Programming/Coroutines/StopCoroutine.md) if you're calling this function while it's already running.  
Similar logic can be applied to all lerp variations, and is commonly used in tweening libraries.

The result is a linear movement from one value to another. If you want to add **easing**, either [`Evaluate`](https://docs.unity3d.com/ScriptReference/AnimationCurve.Evaluate.html) your `t` value with an [`AnimationCurve`](https://docs.unity3d.com/ScriptReference/AnimationCurve.html) (you can configure one via the inspector), or pass it through an [easing function](https://easings.net).

---  
[Return to overview.](Overview.md)
