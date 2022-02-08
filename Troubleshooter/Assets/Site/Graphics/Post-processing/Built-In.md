## Post-processing: Built-in
### Resolution
1. Install the [Post-Processing Stack v2](https://docs.unity3d.com/Packages/com.unity.postprocessing@latest/index.html?subfolder=/manual/Installation.html).
2. Follow the [getting started](https://docs.unity3d.com/Packages/com.unity.postprocessing@latest/index.html?subfolder=/manual/Quick-start.html) instructions.
3. Set the **Post-process Layer**'s **layer** to the layer of the game object the **Post-process Volume** is on.
4. Enable **is global** on the volume if you intend to use it everywhere, otherwise add a trigger collder that the camera must be within.
5. You should typically set the volume's **weight** to 1.
6. Enable the **effect overrides toggle** must be enabled (see [adding post-processing effects to the stack](https://docs.unity3d.com/Packages/com.unity.postprocessing@latest/index.html?subfolder=/manual/Quick-start.html#adding-post-processing-effects-to-the-stack)).
7. Enable the **effect toggle** must be enabled (see [adding post-processing effects to the stack](https://docs.unity3d.com/Packages/com.unity.postprocessing@latest/index.html?subfolder=/manual/Quick-start.html#adding-post-processing-effects-to-the-stack)).
8. Override and modify the parameters associated with an effect to ensure it is visible. Many effects require their intensities to be raised to do this.