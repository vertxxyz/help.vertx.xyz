# Line Renderer: Vertex colors

The colors that make up a Line Renderer are displayed using vertex colors.  
If there are not enough vertices in the line, then not all color stops on the gradient will be represented.

## Resolution
Add more points to increase the number of vertices along the line.  

These points will have to be distributed across the line similarly to the gradient. This can be annoying and difficult to do manually,
so you may want to write a helper script that uses [`Vector3.Lerp`](https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html) to distribute them.
