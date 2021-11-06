## Vertex and Fragment functions
### Description
Shaders are broken up into multiple stages. A typical shader graph is broken up into a Vertex stage, and a Fragment stage (vert, frag).  
The vertex stage controls the position, normals, and tangents, of vertices.  
The fragment stage controls what approximates the pixels on the screen.  
Shader Graph requires the networks that make up these stages to be separated.  
Some nodes are also incompatible with the vertex stage.  

### Resolution
- Do not cross-connect edges across the vert-frag networks. This may require you to duplicate some of your graph.  
- Ensure the vertex stage uses nodes that are compatible with it. Any node contributing to the vertex stage must be compatible.  
    eg. The `Sample Texture 2D` node can only be used in the frag stage. Instead use a `Sample Texture 2D LOD` node.  
    Visit [the documentation](http://docs.unity3d.com/Packages/com.unity.shadergraph@latest/index.html?subfolder=/manual/Node-Library.html), and search for complex nodes in your networks to understand whether they are valid.
- Rebuild the graph node by node from the vert output until something fails to connect to the graph. This node will have to be replaced as it's likely not compatible with the vertex stage.  