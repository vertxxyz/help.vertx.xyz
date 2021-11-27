## Collision messages

Ensure your functions are exactly as below.
The variable `data` can be renamed.  
Type, spelling, and capitalisation are all important.

[OnCollisionEnter](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter.html):
```csharp
void OnCollisionEnter(Collision data)
{
    // Your behaviour here
}
```

[OnCollisionStay](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionStay.html):
```csharp
void OnCollisionStay(Collision data)
{
    // Your behaviour here
}
```

[OnCollisionExit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionExit.html):
```csharp
void OnCollisionExit(Collision data)
{
    // Your behaviour here
}
```

The parameters are optional if you are not using them.

---
[I am still not getting a message.](3%20Collision%20Matrix%203D.md)