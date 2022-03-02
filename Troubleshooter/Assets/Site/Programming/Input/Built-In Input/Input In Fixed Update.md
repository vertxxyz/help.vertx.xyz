## Input in Fixed Update
### Description
If instantaneous [Input](https://docs.unity3d.com/ScriptReference/Input.html) methods (`GetKeyDown`, `GetMouseButtonDown`, etc) are used in `FixedUpdate` or any physics message function (eg. `OnCollisionEnter`) then they will be triggered inconsistently.  

`FixedUpdate` is not guaranteed to run every frame. It is run at a fixed rate, sometimes multiple times a frame, and is frame-rate independent. If an input occurs on a frame where `FixedUpdate` is not run, then it will not be processed by your code.  
See [`FixedUpdate`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html) for more information.  

### Resolution
To resolve this cache values in `Update`, and handle them when they are used in `FixedUpdate` or the appropriate message function.

#### Example
<<Code/Input/Fixed Update Input.rtf>>  

