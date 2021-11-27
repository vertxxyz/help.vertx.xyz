## Basic configuration

Ensure that either:  
- The Audio Source has **Play On Awake** enabled.  
**or**  
- [Play](https://docs.unity3d.com/ScriptReference/AudioSource.Play.html) is called manually.  
You should use the [debugger, or log](../../Programming/Debugging.md) that the function is being called, and the source is correct.
  
Also ensure that the source is not being destroyed, as this will also stop the audio being played.

Make sure that the Audio Clip is actually assigned to the Audio Source.

---
[Audio still cannot be heard.](Missing%20Audio%20Listener.md)