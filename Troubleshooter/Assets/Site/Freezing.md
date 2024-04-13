# Freezing
Freezes are almost always caused by some form of infinite loop. Remove or break out of the loop and the program will resume.

## Resolution
Common forms of undesirable loops include:

::::note
### Incorrect application of a while loop
```csharp
// 🔴 This is an infinite loop
while (Input.GetMouseDown(0))
{
    DoSomething();
}
```
The contents of a while loop is the only thing running until the condition is met.
It's important understand this means **no other code is running**[^1], not even the code that updates input or renders the game.

### In `Update`
Resolve this issue by using an `if` statement instead. As `Update` is already a loop, if the condition is inside it it will be evaluated again the next frame.

```csharp
void Update()
{
    // 🟢 This will only execute once, but because Update is run every frame,
    // it executes again the next frame.
    if (Input.GetMouseDown(0))
    {
        DoSomething();
    }
}
```

### In coroutines
If the loop is inside of a coroutine you must `yield` inside of the loop to cause execution to return to that point later.
[`yield return null`](https://docs.unity3d.com/Manual/Coroutines.html) will return on the next frame. [`yield return new WaitForSeconds(1)`](https://docs.unity3d.com/ScriptReference/WaitForSeconds.html) will return after 1 second.

```csharp
// 🟢 The loop executes forever, but it's not an infinite loop because it's yielded,
// this means it exits when that code runs, and returns later. In this case, for one frame.
while (true)
{
    DoSomething();
    yield return null;
}
```

::::

:::note
### Logic errors in loops
A [functioning IDE](IDE%20Configuration.md) can autocomplete `for` loops by typing <kbd>for</kbd> and pressing tab/enter.
Reverse `for` loops can be created with <kbd>forr</kbd>. This helps prevent basic typing mistakes.

```csharp
for (int x = 0; x < 10; x++)
{
    // 🔴 This is an infinite loop because this inner for loop uses x++ instead of y++.
    // "y" will never reach the condition, and the for loop will never exit.
    // Try to use IDE refactoring tools to rename variables instead of doing it manually.
    for (int y = 0; y < 10; x++)
    {
        DoSomething();
    }
}
```

Modifying the iterator of a loop, or appending to a collection during loop iteration is another common cause for infinite loops.
:::

:::note
### Recursive properties
Calling a property inside of itself can cause an infinite recursive loop that will most often result in a [StackOverflowException](Runtime%20Exceptions/StackOverflowException.md), but can produce an infinite loop.
```csharp
private float _example;
public float Example
{
    get
    {
        // 🔴 This is a recursive loop.
        // Instead of using the backing field "_example", the property's getter is called again.
        if (Example < 10)
            return 0;
        return _example;
    }
}
```
:::

:::note
### Recursive spawning
- Having an object immediately instantiate itself in `Awake` will cause an infinite loop.
- Having a component immediately add itself in `Awake` will cause an infinite loop.
:::

[^1]: Code on background threads may continue running, often until it's forced to wait for your code to complete.
