# BroadcastMessage: No receiver
```
BroadcastMessage X has no receiver!
```

## Check your function is in the class scope
::::note  
Make sure you have declared the function in the correct scope.  
Message functions cannot be called if they are local functions.

### Example
```csharp
public class Example : MonoBehaviour
{
    void Update()
    {
        // This is a local function.
        // 🔴 Don't put your function inside of another.
        void MessageFunction()
        {
            ...
        }
    }

    // 🟢 This is the correct scope for message functions.
    void MessageFunction()
    {
        ...
    }
    
    public void Broadcast() => gameObject.BroadcastMessage(nameof(MessageFunction));
}
```

::::  

## Where possible, use nameof
::::note  
Using `nameof` makes it so your IDE can protect you against spelling mistakes.  
If you do not have access to `nameof` because 
### Example
```csharp
public class Example : MonoBehaviour
{
    // 🟠 Where possible, avoid using strings to refer to message functions.
    public void Broadcast() => gameObject.BroadcastMessage("MessageFunction");
    
    // 🟢 Use nameof, the function is in a scope this code can refer to by name.
    public void Broadcast() => gameObject.BroadcastMessage(nameof(MessageFunction));

    void MessageFunction() { ... }
    
    
}
```

::::  

## Don't require a receiver
::::note  
You can pass [`SendMessageOptions.DontRequireReceiver`](https://docs.unity3d.com/ScriptReference/SendMessageOptions.DontRequireReceiver.html) to [`BroadcastMessage`](https://docs.unity3d.com/ScriptReference/GameObject.BroadcastMessage.html) so it won't print an error when there is no receiver present on the target.

```csharp
gameObject.BroadcastMessage(nameof(MessageFunction), SendMessageOptions.DontRequireReceiver);
```

::::  

## Avoid using BroadcastMessage entirely
::::note  
[`BroadcastMessage`](https://docs.unity3d.com/ScriptReference/GameObject.BroadcastMessage.html) is an expensive and brittle way to send messages between objects.  
Consider using the [`GetComponents`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponents.html) overload that takes a list (avoiding allocating a new array), getting an [interface](https://learn.unity.com/tutorial/interfaces) that defines your message function. Then loop over the results, calling your function.  
::::  
