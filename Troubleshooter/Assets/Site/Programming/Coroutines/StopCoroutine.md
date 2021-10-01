## StopCoroutine
### Description
[StopCoroutine](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html) must be paired with an argument generated from the `StartCoroutine` call.  

### Resolution
Properly cache a `Coroutine` object from the original `StartCoroutine` call.  

<<Code/Coroutines/StopCoroutine.rtf>>

Note that if you are disabling this object you should generally stop the coroutine in [OnDisable](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html) and also set the value to `null` to properly track whether the coroutine is running. When a GameObject is deactivated the coroutine will be stopped; if only the script is disabled it will continue as normal.  

---  

An alternative is to use the [StopAllCoroutines](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopAllCoroutines.html) method that will halt all running coroutines on a MonoBehaviour.