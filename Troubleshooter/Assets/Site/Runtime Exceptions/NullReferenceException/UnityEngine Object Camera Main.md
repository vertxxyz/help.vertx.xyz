# NullReferenceException: UnityEngine.Object — Camera.main

[`Camera.main`](https://docs.unity3d.com/ScriptReference/Camera-main.html) will return `null` if there isn't an enabled camera tagged as <kbd>MainCamera</kbd>.  

## Resolution
[Tag](https://docs.unity3d.com/Manual/Tags.html) the **enabled** camera in your scene you intended to find using the built-in <kbd>MainCamera</kbd> tag.
