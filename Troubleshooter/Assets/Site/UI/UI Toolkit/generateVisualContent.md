## Generate Visual Content

Ensure that:
- Vertices are drawn at the correct depth. The helper [Vertex.nearZ](https://docs.unity3d.com/ScriptReference/UIElements.Vertex-nearZ.html) provides this z depth for you.
- Drawn meshes are using the correct triangle winding order. This is clockwise.
- Colours used for vertices are not transparent.

If these are not your problem, start with the simple case (drawing a triangle/quad) in a basic element and work your way upwards to drawing the entire content.