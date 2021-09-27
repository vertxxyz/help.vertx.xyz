## Asynchronous Shader Compilation
### Description
Unity will render uncompiled shaders with a dummy shader while [Asynchronous Shader compilation](https://docs.unity3d.com/Manual/AsynchronousShaderCompilation.html) is enabled. Disabling this option will cause stalling in the editor, but you will not see the dummy shader.  

:::info
The feature does not have any effect on the standalone Player, because the Editor compiles all the Shader Variants needed by the Player during the build process.
:::