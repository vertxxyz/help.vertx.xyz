# Generate visual content

## Nothing is being drawn
Check that:
- Vertices are drawn at the correct depth. The helper [`Vertex.nearZ`](https://docs.unity3d.com/ScriptReference/UIElements.Vertex-nearZ.html) provides this z depth for you.
- The correct triangle winding order is used. This is clockwise[^1].  
- Colours used for vertices are not transparent.
- Your triangles have area. If you are drawing lines, two vertices isn't enough, you will have to manually give them volume as triangles.
- Your element isn't hidden behind others.
- When using `AllocateTempMesh`, call `DrawMesh` afterward with the vertices and indices slices.

If these are not your problem, start with the simple case (drawing a **large** triangle/quad) in a basic element and work your way upwards to drawing complex content.

## Errors
```
InvalidOperationException: The UNKNOWN_OBJECT_TYPE JobName.Vertices has not been assigned or constructed. All containers must be valid when scheduling a job.
```

You have passed `0` to `AllocateTempMesh`'s count, which has returned an empty span.


[^1]: The coordinate space starts in the top left of the element, with positive coordinates trending to the bottom right.
