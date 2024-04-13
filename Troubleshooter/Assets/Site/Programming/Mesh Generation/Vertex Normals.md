# Vertex normals
The lighting on a mesh considers uses interpolated vertex normals to decide the direction faces point. This enables a mix of hard or soft lighting depending on whether the faces [share vertices or not](Shared%20Vertices.md).

When you distort a mesh without updating the vertex normals it is shading as if it was still flat. You must generate new normals to match the current distortion. You can do this with [`RecalculateNormals`](https://docs.unity3d.com/ScriptReference/Mesh.RecalculateNormals.html) or do it [manually](Generating%20Normals.md).
