## Shader Graph: Sprite Renderer setup
If Shader Graphs are improperly configured, sprites won't update when the Sprite Renderer is changed, and the sprites may become clipped by the current settings.

### Resolution

1. Sprite values should be set via the [Sprite Renderer](https://docs.unity3d.com/Manual/class-SpriteRenderer.html).
1. Use a Sprite Shader Graph (Lit or Unlit)  if it is available to you.
1. Name the [exposed color texture](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/ShaderGraph.html) `MainTex` (the reference should be `_MainTex`).  
   :::warning{.small}  
   This includes capitalisation and spaces  
   :::
   1. Select **+** on the [Blackboard](https://docs.unity3d.com/Packages/com.unity.shadergraph@latest/index.html?subfolder=/manual/Blackboard.html) and then select **Texture 2D**.
   1. Drag the property into the editor window and attach its output to a Sample Texture 2D node.
   1. Attach the RGBA output to the Base Color input of the Fragment block.
   1. Attach the A output to the Alpha input of the Fragment block.