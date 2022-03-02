## Coroutines: Halting
### Description
Coroutines are executing from the MonoBehaviour that started them.  
They are **not** running on the object where the method is, unless that object also ran the associated StartCoroutine call.  
Coroutines can be stopped with the [`StopCoroutine`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html) method, or [`StopAllCoroutines`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopAllCoroutines.html).  
Coroutines are also stopped when the MonoBehaviour is destroyed or if the GameObject the MonoBehaviour is attached to is deactivated.  
Coroutines are **not** stopped when a MonoBehaviour is disabled.  

### Resolution
Ensure you are **not**:  
- Calling `StopAllCoroutines` on the MonoBehaviour running your coroutine.
- Destroying the MonoBehaviour running the coroutine, including the GameObject that contains it.  
   This includes changing scenes and destroying the objects that way.
- Disabling the GameObject containing the MonoBehaviour running the coroutine.
- Using `StopCoroutine` to halt the execution.
- Throwing any exception inside the coroutine.  
   Exceptions will halt the execution of any method, and unless caught will entirely halt a coroutine.

---  
[The coroutine still stops before completion.](Overload.md)