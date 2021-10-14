## Collision messages

Ensure your functions are exactly as below.
The variable `data` can be renamed.  
Type, spelling, and capitalisation are all important.

[OnCollisionEnter2D](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html):
```csharp
void OnCollisionEnter2D(Collision2D data)
{
    // Your behaviour here
}
```

[OnCollisionStay2D](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionStay2D.html):
```csharp
void OnCollisionStay2D(Collision2D data)
{
    // Your behaviour here
}
```

[OnCollisionExit2D](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionExit2D.html):
```csharp
void OnCollisionExit2D(Collision2D data)
{
    // Your behaviour here
}
```

The parameters are optional if you are not using them.  

---
[I am still not getting a message](3%20Collision%20Matrix%202D.md)