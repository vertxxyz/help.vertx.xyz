# Local functions

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
}
```

---
[I am still not getting a message.](5%203D%20Collision%20Matrix.md)
