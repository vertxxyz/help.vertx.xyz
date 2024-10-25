<<Abbreviations/SRP.md>>
# Shader errors with SRPs
The common problem people have with SRPs is improper setup. This is when the SRP asset isn't assigned in Project Settings.  

Setup for **URP** can be found [here](https://docs.unity3d.com/Manual/urp/InstallingAndConfiguringURP.html), upgrading shaders [here](https://docs.unity3d.com/Manual/urp/upgrading-your-shaders.html).  
Setup for **HDRP** can be found [here](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/Upgrading-To-HDRP.html).  

Not all shaders are SRP compatible. In most cases if you are using custom shaders that cannot be upgraded, you should recreate the shader using [Shader Graph](https://docs.unity3d.com/Packages/com.unity.shadergraph@latest).  
For URP Cyan has a great writeup on writing custom shaders found [here](https://cyangamedev.wordpress.com/2020/06/05/urp-shader-code/).

---
If your material is still pink, see [General Shader Errors](General%20Shader%20Errors.md) for further debugging.
