## Input in Fixed Update
### Description
When **instantaneous** [`Input`](https://docs.unity3d.com/ScriptReference/Input.html) methods ([`GetKeyDown`](https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html), [`GetMouseButtonDown`](https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html), and others) are used in `FixedUpdate` or any physics message function (`OnCollisionEnter` for example) they will be triggered inconsistently.  

#### Why?
`FixedUpdate` is run at a fixed rate, sometimes multiple times a frame, and is frame-rate independent. If input occurs on a frame where `FixedUpdate` isn't run, it won't be processed by your code.  
See [`FixedUpdate`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html) for more information.  

### Resolution
Cache values in [`Update`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html), and consume them in `FixedUpdate` or appropriate message functions.

#### Example
<<Code/Input/Fixed Update Input.rtf>>  

---  

When using physics callbacks and not calling physics functions, you can move your logic to `Update`.

#### Example

<<Code/Input/Physics Message Input.rtf>>

If you expect overlapping triggers, this logic should involve a counter.