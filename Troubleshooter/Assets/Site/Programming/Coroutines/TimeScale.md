## Coroutines: Halting
### Description
If [`Time.timeScale`](https://docs.unity3d.com/ScriptReference/Time-timeScale.html) is set to zero, coroutines suspended using [`WaitForSeconds`](https://docs.unity3d.com/ScriptReference/WaitForSeconds.html) will not be called.
### Resolution
Replace [`WaitForSeconds`](https://docs.unity3d.com/ScriptReference/WaitForSeconds.html) with [`WaitForSecondsRealtime`](https://docs.unity3d.com/ScriptReference/WaitForSecondsRealtime.html) if you want to ignore scaled time.

---  
[The coroutine still stops before completion.](Overload.md)