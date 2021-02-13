<<Abbreviations/SRP.md>>
### Diffusion Profile
In HDRP the default diffusion profile has a green tint to indicate it has not been properly set up.  
Follow the instructions to properly set up a diffusion profile [here](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/Diffusion-Profile.html).

---
If you are not using HDRP, this is likely caused by a lighting issue, or bad logic in a custom shader. If your scene involves baked assets like lightmaps or reflection probes, try rebaking lighting data.