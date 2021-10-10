## Trigger Messages

Ensure your functions are exactly as below.
The variable `data` can be renamed.  
Type, spelling, and capitalisation are all important.

[OnTriggerEnter2D](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html):
```csharp
void OnTriggerEnter2D(Collider2D data)
{
    // Your behaviour here
}
```

[OnTriggerStay2D](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerStay2D.html):
```csharp
void OnTriggerStay2D(Collider2D data)
{
    // Your behaviour here
}
```

[OnTriggerExit2D](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerExit2D.html):
```csharp
void OnTriggerExit2D(Collider2D data)
{
    // Your behaviour here
}
```

The parameters are optional if you are not using them.

---
[I am still not getting a message](3%20Trigger%20Matrix%202D.md)