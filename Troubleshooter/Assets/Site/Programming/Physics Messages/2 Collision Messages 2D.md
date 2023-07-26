## Collision messages (2D)

Ensure your functions use the correct **types**, **spelling**, and **capitalisation** as below.  
The variable `collision` can be renamed, and [access modifiers](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) are not important. The parameters are optional if you are not using them.

### [`OnCollisionEnter2D`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html)
```csharp
void OnCollisionEnter2D(Collision2D collision)
{
    // Your behaviour here
}
```

### [`OnCollisionStay2D`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionStay2D.html)
```csharp
void OnCollisionStay2D(Collision2D collision)
{
    // Your behaviour here
}
```

### [`OnCollisionExit2D`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionExit2D.html)
```csharp
void OnCollisionExit2D(Collision2D collision)
{
    // Your behaviour here
}
```

---
[I am still not getting a message.](3%20Collision%20Matrix%202D.md)