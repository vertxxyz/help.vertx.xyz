# Coroutines: Halting

Coroutines are executing from the MonoBehaviour that started them.  
They are **not** running on the object where the method is, unless that object also ran the associated StartCoroutine call.  
Coroutines can be stopped with the [`StopCoroutine`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html) method, or [`StopAllCoroutines`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopAllCoroutines.html).  
Coroutines are also stopped when the MonoBehaviour is destroyed or if the GameObject the MonoBehaviour is attached to is deactivated.  
Coroutines are **not** stopped when a MonoBehaviour is disabled.  

## Resolution
### Ensure you don't:  
- Call `StopAllCoroutines` on the MonoBehaviour after running your coroutine.
- Destroy the MonoBehaviour running the coroutine or the GameObject that contains it.
- Change scenes (destroying GameObjects). Persistent scenes or DontDestroyOnLoad will still run.
- Deactivate the GameObject the MonoBehaviour running the coroutine is added to.
- Use `StopCoroutine` to halt the execution.
- Throw any exception inside the coroutine.  
   Exceptions halt the execution of any method, and unless caught will entirely halt a coroutine.

---  
[The coroutine still stops before completion.](TimeScale.md)
