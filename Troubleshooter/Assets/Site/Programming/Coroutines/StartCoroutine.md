## Coroutines: Halting
### Description

Coroutines must be started properly to run to completion.  
Starting a coroutine like a normal method call will halt silently.  

### Resolution
Properly start the coroutine with a call to [StartCoroutine](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html). You are capable of passing arguments to the coroutine using this method.

<<Code/Coroutines/StartCoroutine.rtf>>

---  
[The coroutine still stops before completion](Disabling%20Objects.md)