## Generate visual content

Ensure that:
- Vertices are drawn at the correct depth. The helper [`Vertex.nearZ`](https://docs.unity3d.com/ScriptReference/UIElements.Vertex-nearZ.html) provides this z depth for you.
- The correct triangle winding order is used. This is clockwise[^1].  
- Colours used for vertices are not transparent.
- Your triangles have area. If you are drawing lines, two vertices is not enough, you will have to manually give them volume as triangles.
- Your element is not hidden behind others.

If these are not your problem, start with the simple case (drawing a **large** triangle/quad) in a basic element and work your way upwards to drawing complex content.

[^1]: The coordinate space starts in the top left of the element, with positive coordinates trending to the bottom right