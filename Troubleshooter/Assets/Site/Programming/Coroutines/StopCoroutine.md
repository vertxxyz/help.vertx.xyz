## StopCoroutine

[`StopCoroutine`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html) must be paired with an argument generated from the [`StartCoroutine`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html) call.  

### Resolution
To stop a coroutine, cache the `Coroutine` object returned by the original `StartCoroutine` call, and pass it to `StopCoroutine`.  

#### Example
<<Code/Coroutines/StopCoroutine.rtf>>

::::note
:::info{.inline}
When a GameObject is deactivated a coroutine will be stopped, but if the script is disabled it will continue.  
:::

When disabling this object you should generally stop the coroutine in [`OnDisable`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html) and set the value to `null` to track whether the coroutine is running. 
::::  

---  

An alternative is to use the [`StopAllCoroutines`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopAllCoroutines.html) method that halts all running coroutines on a MonoBehaviour.