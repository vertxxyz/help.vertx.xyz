## Trigger messages

Ensure your functions are exactly as below.
The variable `data` can be renamed.  
Type, spelling, and capitalisation are all important.

[OnTriggerEnter](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter.html):
```csharp
void OnTriggerEnter(Collider data)
{
    // Your behaviour here
}
```

[OnTriggerStay](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerStay.html):
```csharp
void OnTriggerStay(Collider data)
{
    // Your behaviour here
}
```

[OnTriggerExit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerExit.html):
```csharp
void OnTriggerExit(Collider data)
{
    // Your behaviour here
}
```

The parameters are optional if you are not using them.

---
[I am still not getting a message.](3%20Trigger%20Matrix%203D.md)