<<Abbreviations/SRP.md>>
## Render Pipeline compatibility
### Description
Shader Graph is only compatible with Universal Rendering Pipeline (formerly LWRP), and High Definition Rendering Pipeline.  
The built-in render pipeline **does not** support Shader Graph.

### Resolution

- Switch render pipelines to use Shader Graph. This is generally advisable at the start of a project with an understanding of the repercussions associated with using one.
    - [Switch to URP](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/InstallingAndConfiguringURP.html)  
      A light-weight pipeline set to replace the built-in pipeline.
    - [Switch to HDRP]( https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest/index.html?subfolder=/manual/Upgrading-To-HDRP.html)  
      A complex high-fidelity pipeline intended for modern PCs and Consoles.
- Use an alternate solution.  
    - [Amplify Shader Editor](https://assetstore.unity.com/packages/tools/visual-scripting/amplify-shader-editor-68570)  
      A fully-featured and supported shader editor on the Asset Store.