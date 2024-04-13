# InvalidCastException

Invalid casts occur when an instance is converted to a type it does not match.

## Resolution

Type casting looks like this:
```csharp
var end = (Type)start;
```
and is called a [cast expression](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-cast#cast-expression).

:::note  
### I am using a cast expression
Understand the types involved in your expression.
The object must inherit the type it's being casted to, or must have a [user-defined conversion](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators) to that type.  
:::  

:::note  
### I am using `Instantiate`
If [`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html) is throwing the exception, then the original type of the object has been changed.
Re-assign the object you are instancing in the Inspector to fix the type mismatch.  
:::  

:::note  
### I am using a foreach
If you are using a foreach with `Transform` then you must use the `Transform` type for the elements. No other type will work.

```csharp
// 🔴 Incorrect, Example is not a Transform type.
foreach(Example item in transform)
    ...

// 🟢 Correct, the type used is Transform
foreach(Transform item in transform)
    ...
```

:::  

---

If your issue was not listed, please <<report-issue.html>> so this page can be improved.
