## StackOverflowException

A StackOverflowException most often occurs in recursive cases, where code is calling itself in an infinite loop.  
In this case the stack—a place where memory local to a scope is allocated—grows until there is no more space to contain it.

### Resolution
Fix the recursive loop.  

The [stack trace](../Stack%20Traces.md) should give a very good indicator of the path of the recursive loop. Look for repetition in the trace, and look to remove the cause of the loop.

:::note
#### Recursive methods
Recursion is a helpful tool, but as with any loop can easily introduce logic errors, causing it to run infinitely.
```csharp
public void DoSomethingRecursively(int iteration)
{
    if (iteration > 100) return;
    // If this method was called again before the condition was checked,
    // or if the iteration parameter was not increased,
    // then this loop will run until the stack overflows.
    DoSomethingRecursively(iteration + 1);
}
```

In some other cases recursive loops can hit the limit of the stack without the loop being infinite. In these cases you may have to switch the logic to be a more standard loop—like a while loop—so the stack doesn't grow on each loop iteration.  
:::  

:::note
#### Recursive properties
Calling a property inside of itself can cause an infinite recursive loop.
```csharp
private float _example;
public float Example
{
    get
    {
        // This is a recursive loop.
        // Instead of using the backing _example, the property's getter is called again.
        if (Example < 10)
            return 0;
        return _example;
    }
}
```
:::

### Notes
Extracting stack traces can become extremely costly in recursive situations as the size grows. Note that logging with stack traces enabled can become a bottleneck in these situations.