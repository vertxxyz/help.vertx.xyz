# ArgumentNullException

An `ArgumentNullException` is thrown when what is passed to a method is `null`.

```csharp
Foo bar = null;
// If the method doesn't accept null it may throw an ArgumentNullException.
Method(bar);
```

The **parameter name** listed in the error, and the [stack trace](../Stack%20Traces.md), are both clues to what is `null`.

### Extension methods
Be aware that [extension methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) can throw this exception when the object you invoke the method on is invalid or `null`.
- [`Texture2D.EncodeToPNG();`](https://docs.unity3d.com/ScriptReference/ImageConversion.EncodeToPNG.html) will throw an exception if `Texture2D` is `null`.

### Crossover with Unity-null
You can run methods on [unity-null](../Unity%20Null.md) objects because they actually still exist. If the method doesn't implement internal checks as expected, this can manifest as an `ArgumentNullException` instead of a typical `NullReferenceException`.
 
```csharp
AudioSource source = GetComponent<AudioSource>();
// If source was null it will throw an ArgumentNullException.
source.PlayOneShot(clip, 1);
``` 

- [`AudioSource.PlayOneShot()`](https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html) and [`AudioSource.Play()`](https://docs.unity3d.com/ScriptReference/AudioSource.Play.html) will throw an exception if `AudioSource` is `null`.

### From internal Unity code
Note that many errors from Unity Editor code can throw an `ArgumentNullException`, if the stack trace doesn't contain user code then it's likely not _your_ issue, and Unity may just need to be restarted.

---

After understanding this page's content, visit [`NullReferenceException`](NullReferenceException.md) to further debug your issue.
