## Coroutines: Waiting

Starting a coroutine does not cause your code to wait for the coroutine to finish running.  
A coroutine is started, execution runs until the first `yield`, then execution returns to the code after the StartCoroutine call, and continues. When the yield is resolved the execution will return to that point. Waiting only affects code running inside the coroutine.  

### Resolution
🔴 Instead of:
```csharp
public void Example()
{
    StartCoroutine(MyCoroutine(5));
    // This logic is incorrect
    // Code run here will execute on the same frame.
    DoSomethingAfter5Seconds();
}

private IEnumerator MyCoroutine(float waitTime)
{
    yield new WaitForSeconds(waitTime);
}
```
🟢 Move your code inside the coroutine:
```csharp
public void Example()
{
    StartCoroutine(MyCoroutine(5));
}

private IEnumerator MyCoroutine(float waitTime)
{
    yield new WaitForSeconds(waitTime);
    DoSomethingAfter5Seconds();
}
```