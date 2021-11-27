## 3D audio troubleshooting
First, move the **Spacial Blend** slider on your Audio Source entirely to 2D.  
If audio can now be heard, then either your Audio Listener is not inside the Audio Source's volume, or the 3D Sound Settings are incorrect.  
You can find the Audio Listener by searching `t:AudioListener` in the Hierarchy view. Ensure that this transform is inside the sphere gizmos that are shown when selecting the Audio Source. Outside the outermost sphere is 0% volume, and the innermost is 100%. These are the **Min Distance** and **Max Distance** settings in the **3D Sound Settings** dropdown.

If your AudioSource has Spacial Blend set to **3D** then by default the falloff of the audio is **Logarithmic**, this is a semi-realistic approach where audio fades out faster as you leave the immediate area. You can see this influence under the 3D Sound Settings dropdown, with the vertical line being the position of the audio listener, and the red line being the volume rolloff.  
Switching to Linear or Custom may be appropriate if you wish to achieve a more specific result, but increasing the Min Distance can make a meaningful difference.

---
[Audio still cannot be heard.](AudioSource%20Play.md)