## Enable toggle: Lifetime functions
### Description
Disabling a script only affects a few functions implemented by that component. A select few of those functions are required to enable the toggle in the component inspector.

### Resolution
Add any of the listed functions:

- [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html)
- [`OnEnable`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnEnable.html)
- [`OnDisable`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html)
- [`Update`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html)
- [`LateUpdate`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html)
- [`FixedUpdate`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html)
- [`OnGUI`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html)

If you do not need one of these functions but want the checkbox, add `Start`, as it's only called once in a script's lifetime.