## Anonymous methods and closures
### Premise
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

#### Why is this?
What the code really looks like when compiled is this:  
```csharp
[CompilerGenerated]
private sealed class <>c__DisplayClass0_0
{
    public int i;

    internal void <ExampleA>b__0()
    {
        Debug.Log(i);
    }
}

public void Example()
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
```

Looking past all the fancy symbols, you can make out a DisplayClass whose variable `i` is used as the counter to our loop.  
That class is shared across all Actions we create, and `i` is increased to `10` over the loop's iterations.

### The fix
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

#### Why does this work?

The new version of the compiled code looks like this:
```csharp
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
```

As you can see, now a new DisplayClass is created in every iteration of the loop, and the counter is copied to that instance. Meaning only `num` is increased to `10`, and each `iLocal` is a copy of the state at a different iteration of the loop.