## StopCoroutine

:::error
[`StopCoroutine`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html) must be paired with an argument generated from the [`StartCoroutine`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html) call.
:::

### Stopping coroutines from outside
To stop a coroutine, cache the `Coroutine` object returned by the original `StartCoroutine` call, and pass it to `StopCoroutine`.

```csharp
_exampleCoroutine = StartCoroutine(ExampleCoroutine());
...
StopCoroutine(_exampleCoroutine);
```

::::note
#### Example
^^^
```csharp
// Create a field to cache a reference to our coroutine.
private Coroutine _exampleCoroutine;

void StartExample()
{
    // Cache the Coroutine returned from StartCoroutine.
    _exampleCoroutine = StartCoroutine(ExampleCoroutine());
}

void StopExample()
{
    if (_exampleCoroutine == null)
    {
        // If there was no coroutine, there is nothing to stop.
        return;
    }

    // Cancel the Coroutine we previously cached.
    StopCoroutine(_exampleCoroutine);

    // Release the reference for tracking whether the coroutine is running.
    _exampleCoroutine = null;
}

IEnumerator ExampleCoroutine()
{
    /* Collapsable: Example logic */
    // Example logic
    yield return new WaitForSeconds(1);
    Debug.Log("Complete!");
    /* End Collapsable */

    // Release the reference for tracking whether the coroutine is running.
    _exampleCoroutine = null;
}

/* Collapsable: Example helper methods */
// Because we set _exampleCoroutine to null every time the coroutine ends,
// we can use it to check whether the coroutine is running.
bool IsExampleCoroutineRunning() => _exampleCoroutine != null;
/* End Collapsable */
```
^^^ ::`StopCoroutine` will throw a [`NullReferenceException`](../Runtime%20Exceptions/NullReferenceException.md) if `null` is passed to it.::{.error}
::::

#### When disabling scripts

:::info
When a **GameObject** is deactivated its coroutine will be stopped, but if the **script** is disabled they will continue.
:::

You may want to stop the coroutine in [`OnDisable`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html).

```csharp
// In our example we can just call StopExample.
void OnDisable() => StopExample();
```

#### Alternatives

An alternative is to use the [`StopAllCoroutines`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopAllCoroutines.html) method that halts all running coroutines on a `MonoBehaviour`.

### Stopping coroutines from within
`yield break` will exit a coroutine early.

Coroutines will naturally stop when execution reaches the end of the function.
