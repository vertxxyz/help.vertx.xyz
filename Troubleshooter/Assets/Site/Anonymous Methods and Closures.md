# Anonymous methods and closures
Say you have the code:
```csharp
public void ExampleA()
{
    var actions = new Action[10];
    for (int i = 0; i < 10; i++)
    {
        actions[i] = () => Debug.Log(i);
    }
}
```
When the logs in `actions` are invoked after this loop there will be 10 logs of `10`.  

### Why does this occur?{.foldout}

What the code really looks like when compiled is this:  
```csharp
// Lowered C# code:

[CompilerGenerated]
private sealed class <>c__DisplayClass0_0
{
    public int i;

    internal void <ExampleA>b__0()
    {
        Debug.Log(i);
    }
}

public void ExampleA()
{
    Action[] array = new Action[10];
    <>c__DisplayClass0_0 <>c__DisplayClass0_ = new <>c__DisplayClass0_0();
    <>c__DisplayClass0_.i = 0;
    while (<>c__DisplayClass0_.i < 10)
    {
        array[<>c__DisplayClass0_.i] = new Action(<>c__DisplayClass0_.<ExampleA>b__0);
        <>c__DisplayClass0_.i++;
    }
}

// Lowered C# code ⚠️ manually simplified ⚠️:

private sealed class DisplayClass
{
    public int i;

    internal void B() => Debug.Log(i);
}

public void ExampleA()
{
    var array = new Action[10];
    DisplayClass displayClass = new();
    displayClass.i = 0;
    while (displayClass.i < 10)
    {
        array[displayClass.i] = new Action(displayClass.B);
        displayClass.i++;
    }
    // displayClass.i is 10
}
```

Looking past the fancy symbols (see the manually simplified code), you can see a `DisplayClass` whose variable `i` is used as the counter to our loop.  
That class is shared across all Actions we create, and `i` is increased to `10` over the loop's iterations.  
When the delegate is invoked after the loop, that value, `10`, is used.

## Resolution
Redeclare a local version of the counter, using it in the delegate:
```csharp
public void ExampleB()
{
    var actions = new Action[10];
    for (int i = 0; i < 10; i++)
    {
        int iLocal = i;
        actions[i] = () => Debug.Log(iLocal);
    }
}
```

### Why does this work?{.foldout}

The new version of the compiled code looks like this:
```csharp
// Lowered C# code:

[CompilerGenerated]
private sealed class <>c__DisplayClass0_0
{
    public int iLocal;

    internal void <ExampleB>b__0()
    {
        Debug.Log(iLocal);
    }
}

public void ExampleB()
{
    Action[] array = new Action[10];
    int num = 0;
    while (num < 10)
    {
        <>c__DisplayClass0_0 <>c__DisplayClass0_ = new <>c__DisplayClass0_0();
        <>c__DisplayClass0_.iLocal = num;
        array[num] = new Action(<>c__DisplayClass0_.<ExampleB>b__0);
        num++;
    }
}

// Lowered C# code ⚠️ manually simplified ⚠️:

private sealed class DisplayClass
{
    public int iLocal;

    internal void B()
    {
        Debug.Log(iLocal);
    }
}

public void ExampleB()
{
    var array = new Action[10];
    int num = 0;
    while (num < 10)
    {
        // A new DisplayClass is created in the loop, and iLocal is never increased.
        DisplayClass displayClass = new();
        displayClass.iLocal = num;
        array[num] = new Action(displayClass.B);
        num++;
    }
}
```

As you can see, now a new `DisplayClass` is created in every iteration of the loop, and the counter is copied to that instance. Meaning only `num` is increased to `10`, and each `iLocal` is a copy of the state at a different iteration of the loop.

---

If you use [JetBrains Rider](https://www.jetbrains.com/lp/dotnet-unity/) you can avoid this issue as it will show the warning:
:::warning{.small}
Captured variable is modified in the outer scope
:::

## How to enforce no closures
This isn't always applicable, but certain methods like UI Toolkit's [`RegisterCallback`](https://docs.unity3d.com/ScriptReference/UIElements.CallbackEventHandler.RegisterCallback.html) take in an args parameter, using generics to capture variables in pooled classes.  
If you're using a delegate that expects not to capture variables, you can mark it with the `static` keyword (as of C# 9.0).

### Example
```csharp
field.RegisterCallback<ClickEvent, VisualElement>(
    static (_, element) => element.Focus(),
    otherElement
);
```
