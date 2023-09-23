## Trigger messages (2D)

Ensure your functions use the correct **types**, **spelling**, and **capitalisation** as below.  
The variable `collision` can be renamed, and [access modifiers](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) are not important. The parameters are optional if you are not using them.

### [`OnTriggerEnter2D`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html)
```csharp
void OnTriggerEnter2D(Collider2D collider)
{
    // Your behaviour here
}
```

### [`OnTriggerStay2D`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerStay2D.html)
```csharp
void OnTriggerStay2D(Collider2D collider)
{
    // Your behaviour here
}
```

### [`OnTriggerExit2D`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerExit2D.html)
```csharp
void OnTriggerExit2D(Collider2D collider)
{
    // Your behaviour here
}
```

---
[I am still not getting a message.](3%20Trigger%20Matrix%202D.md)