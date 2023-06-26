## Animator hash does not exist

[`StringToHash`](https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html) returns an `int` parameter for use in Animator methods that would otherwise take a `string`.  
The string argument must match an ID used by the Animator.

### Resolution
Make sure the argument passed to `StringToHash` is **identical** to one used in the Animator.  

:::warning
IDs are **case sensitive**.
:::  