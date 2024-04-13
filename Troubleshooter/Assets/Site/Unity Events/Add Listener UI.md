# UnityEvents - AddListener

Callbacks added via [`AddListener`](https://docs.unity3d.com/ScriptReference/Events.UnityEvent.AddListener.html) do not show up in the UI.  

## Resolution
### Runtime
This is the intended behaviour. Dynamically added callbacks will not be shown in the UI as it is intended for authoring only.

### Editor
If you are writing Editor-only code you can use the [`UnityEventTools`](https://docs.unity3d.com/ScriptReference/Events.UnityEventTools.html) functions to register persistent listeners. These are the callbacks that appear in the UI.
