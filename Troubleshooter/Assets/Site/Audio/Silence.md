---
title: "Audio cannot be heard"
description: "Troubleshooting audio in Unity"
---
# Audio cannot be heard

1. [Muted audio](#muted-audio)
1. [Global volume](#global-volume)
1. [3D audio troubleshooting](#d-audio-troubleshooting)
1. [Basic configuration](#basic-configuration)
1. [Missing Audio Listener](#missing-audio-listener)
1. [Unity audio is disabled](#unity-audio-is-disabled)
1. [System volume](#system-volume)

## Muted audio
Check that **Mute Audio** isn't toggled on in the **Game view**.

![Mute Audio Toggle](mute-toggle.png)

## Global volume
**Global volume** should not be zero in **Edit | Project Settings | Audio**.

## Basic configuration
1. Check that either:
- The Audio Source has **Play On Awake** enabled.  
  **or**
- [`Play`](https://docs.unity3d.com/ScriptReference/AudioSource.Play.html) is called manually.
  You should use the [debugger, or log](../../Debugging.md) that the function is being called, and the source is correct.
  ::::warning
  Calling `Play` repeatedly will cause the clip to constantly restart.  
  This may appear as silence.
  ::::
2. Make sure that the Audio Clip is actually assigned to the Audio Source.
3. Check that the source isn't being destroyed, as this will also stop the audio being played.

## 3D audio troubleshooting
First, move the **Spacial Blend** slider on your Audio Source entirely to 2D.  
If audio can now be heard, then either your Audio Listener isn't inside the Audio Source's volume, or the 3D Sound Settings are incorrect.  
You can find the Audio Listener by searching `t:AudioListener` in the Hierarchy view. Check that this transform is inside the sphere gizmos that are shown when selecting the Audio Source. Outside the outermost sphere is 0% volume, and the innermost is 100%. These are the **Min Distance** and **Max Distance** settings in the **3D Sound Settings** dropdown.

If your AudioSource has Spacial Blend set to **3D** then by default the falloff of the audio is **Logarithmic**, this is a semi-realistic approach where audio fades out faster as you leave the immediate area. You can see this influence under the 3D Sound Settings dropdown, with the vertical line being the position of the audio listener, and the red line being the volume rolloff.  
Switching to Linear or Custom may be appropriate if you wish to achieve a more specific result, but increasing the Min Distance can make a meaningful difference.

## Missing Audio Listener
There must be an AudioListener in the Scene.  
When searching `t:AudioListener` in the Hierarchy view in Play Mode there should be a single result.  
If there are no Audio Listeners then you need to add the component (generally they are present on the main Camera).  
If you have multiple, this can cause other issues, removing the second listener is required.  

## Unity audio is disabled
Check that **Disable Unity audio** isn't checked in **Edit | Project Settings | Audio**.

## System volume
Check that:
1. You can hear audio output from other applications.
1. Unity's volume in your system's volume mixer is loud enough.
1. The output volume of your PC's speakers is loud enough. 

---
[Return to audio issues](Common%20Issues.md)
