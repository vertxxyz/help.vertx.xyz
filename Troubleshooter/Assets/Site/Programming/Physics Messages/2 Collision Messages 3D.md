## Collision messages (3D)

Ensure your functions are exactly as below.  
Types, spelling, and capitalisation matter. The variable `collider` can be renamed, and [access modifiers](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) are not important.

### [`OnCollisionEnter`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter.html)
```csharp
void OnCollisionEnter(Collision collision)
{
    // Your behaviour here
}
```

### [`OnCollisionStay`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionStay.html)
```csharp
void OnCollisionStay(Collision collision)
{
    // Your behaviour here
}
```

### [`OnCollisionExit`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionExit.html)
```csharp
void OnCollisionExit(Collision collision)
{
    // Your behaviour here
}
```

:::info{.inline}  
The parameters are optional if you are not using them.
:::

---
[I am still not getting a message.](3%20Collision%20Matrix%203D.md)