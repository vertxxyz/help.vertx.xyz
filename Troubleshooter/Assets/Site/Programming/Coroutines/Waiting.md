## Coroutines: Waiting

Starting a coroutine does not cause your code to wait for the coroutine to finish running, it is not blocking.  

When a coroutine is started execution runs until the first `yield`, then execution returns to the code after the [`StartCoroutine`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html) call. When the `yield` resolves, execution will return there, continuing until the next `yield` or the end of the coroutine.  

Waiting only affects code running inside the coroutine, or another that yields for it.  

### Resolution
#### 🔴 Instead of
```csharp
public void Example()
{
    StartCoroutine(MyCoroutine(5));
    
    // This logic is incorrect.
    // Code run here will execute on the same frame.
    DoSomethingAfterWaiting();
}

private IEnumerator MyCoroutine(float waitTime)
{
    yield new WaitForSeconds(waitTime);
}
```
#### 🟢 Move your code inside the coroutine
```csharp
public void Example()
{
    StartCoroutine(MyCoroutine(5));
}

private IEnumerator MyCoroutine(float waitTime)
{
    yield new WaitForSeconds(waitTime);
    
    // This code runs after the yield has resolved.
    DoSomethingAfterWaiting();
}
```

---

- [Return to Coroutines.](../Coroutines.md)