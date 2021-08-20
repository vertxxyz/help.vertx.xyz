### StopCoroutine
#### Description
[StopCoroutine](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html) must be paired with an argument generated from the `StartCoroutine` call.  

#### Resolution
Properly cache a `Coroutine` object from the original `StartCoroutine` call.  

<<Code/Coroutines/StopCoroutine.rtf>>

---  

An alternative is to use the [StopAllCoroutines](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopAllCoroutines.html) method that will halt all running coroutines on a MonoBehaviour.