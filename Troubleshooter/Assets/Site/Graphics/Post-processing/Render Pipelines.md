# Post-processing: Render pipelines
Both URP and HDRP have their own built-in post-processing stacks.  

## Resolution
If you have a **Post Process Layer** component on your camera, or a **Post Process Volume** in your scene, you are using the wrong post-processing.  

### Replace the Post Processing V2 package
1. Remove the **Post Process Layer** and **Post Process Volume** components.
2. Remove the Post Processing package from the Package Manager (**Window | Package Manager**).
3. Replace previous Post Process Volume components with the **Volume** component. ([URP](https://docs.unity3d.com/Manual/urp/volumes-landing-page.html), [HDRP](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/Volumes.html))
4. Create a new profiles through the Volume component's interface.

### Enable post-processing
1. Enable **post processing** on the Camera.
2. Enable **post processing** on the Renderer used by the active Render Pipeline Asset.
3. Set the volume's **mode** to global if you intend to use it everywhere, otherwise add a trigger collider that the camera must be within.
4. You should typically set the volume's **weight** to 1.
5. Enable the toggle associated with an effect, then override and modify its parameters to ensure it is visible. Many effects require raised intensities to see them.
