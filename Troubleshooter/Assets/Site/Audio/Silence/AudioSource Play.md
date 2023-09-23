## Basic configuration
#### 1. Check that either:
- The Audio Source has **Play On Awake** enabled.
**or**
- [`Play`](https://docs.unity3d.com/ScriptReference/AudioSource.Play.html) is called manually.
  You should use the [debugger, or log](../../Debugging.md) that the function is being called, and the source is correct.
  ::::warning{.small}
  Calling `Play` repeatedly will cause the clip to constantly restart.
  ::::

#### 2. Make sure that the Audio Clip is actually assigned to the Audio Source.

#### 3. Check that the source isn't being destroyed, as this will also stop the audio being played.

---
[Audio still cannot be heard.](Missing%20Audio%20Listener.md)
