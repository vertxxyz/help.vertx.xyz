---
title: "CS0246: Other considerations"
description: "The type or namespace name 'Foo' could not be found (are you missing a using directive or an assembly reference?)"
---
# [CS0246](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0246): Other considerations

```
The type or namespace name 'Foo' could not be found (are you missing a using directive or an assembly reference?)
```

## Either
::::note
### You have an IDE issue
Save, and check if there are compiler errors in Unity's [Console window](https://docs.unity3d.com/Manual/Console.html) (**Window | General | Console**, <kbd>Ctrl+Shift+C</kbd>).  
**If there are no errors listed**, then the issue isn't real, and your IDE needs to be refreshed.
1. If it's present, select **regenerate project files** in **Edit | Preferences | External Tools**.
1. Restart your IDE.

::::  

::::note
### You have misspelt the type or namespace
Make sure you follow the autocomplete suggestions of your IDE and do not type things manually.  
:::info{.small}
If your IDE isn't showing errors, you will need to [configure your IDE](../IDE%20Configuration.md).  
:::

::::  

::::note
### Your code is incomplete and the type was not provided
```csharp
// 🔴 Incorrect. A variable requires both a type and a name.
public void Example(myVariable)
{
    ...
}
```

:::info{.small}
If your IDE isn't showing errors, you will need to [configure your IDE](../IDE%20Configuration.md).  
:::

::::

::::note
### You are missing the full path to the type
If the type is nested in another class you must specify the outer class to access the inner.
`OuterClass.InnerClass` for example.  
::::

::::note
### The type doesn't exist
The type may have changed name, or doesn't exist in the version of Unity or the package you are trying to reference.
Check the documentation, ensuring you are looking at the correct version.

::::

---
Sorry, we've run out of troubleshooting!
If you resolved your issue and the fix was not listed in the [troubleshooting steps](CS0246.md), please <<report-issue.html>>.
