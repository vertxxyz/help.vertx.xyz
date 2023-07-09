## Trigger messages (3D)

Ensure your functions are exactly as below.
The variable `collider` can be renamed, and [access modifiers](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) are not important.  
Type, spelling, and capitalisation are all important.

[OnTriggerEnter](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter.html):
```csharp
void OnTriggerEnter(Collider collider)
{
    // Your behaviour here
}
```

[OnTriggerStay](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerStay.html):
```csharp
void OnTriggerStay(Collider collider)
{
    // Your behaviour here
}
```

[OnTriggerExit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerExit.html):
```csharp
void OnTriggerExit(Collider collider)
{
    // Your behaviour here
}
```

The parameters are optional if you are not using them.

---
[I am still not getting a message.](3%20Trigger%20Matrix%203D.md)