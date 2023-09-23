## Generating normals

[`RecalculateNormals`](https://docs.unity3d.com/ScriptReference/Mesh.RecalculateNormals.html) is not robust when double-sided meshes that share vertices between front and back faces are used. It cannot properly determine which triangles should be used to generate the vertex normals in that case.

### Implementation

To properly calculate normals when multiple triangles share the same vertices you must selectively generate the normals based on the triangles you care about.

Inigo Quilez has a nice article on [generating mesh normals](https://iquilezles.org/articles/normals/) that I recommend reading. However, you can calculate normals using the example code listed below.
#### Example

```csharp
normals = new Vector3[vertices.Length];

// This for loop should be adjusted to only process the triangles you actually want to contribute towards the final normals.
// For example, triangleLength should be half the length of the triangles array if you have an extra set of reversed triangle indices for the back faces.
for (int t = 0; t < triangleLength; t += 3)
{
    int i0 = triangles[t];
    int i1 = triangles[t + 1];
    int i2 = triangles[t + 2];
    Vector3 a = vertices[i0];
    Vector3 b = vertices[i1];
    Vector3 c = vertices[i2];
    Vector3 n = Vector3.Cross(b - a, c - a);
    normals[i0] += n;
    normals[i1] += n;
    normals[i2] += n;
}

for (var i = 0; i < normals.Length; i++)
    normals[i].Normalize();
```

---

If you are still having issues with your triangles, perhaps the vertex normals are correct, but you have duplicated triangles. Do some [visual debugging](../../Debugging/Draw%20Functions.md) and check that the [winding order](Winding%20Order.md) of your triangles are correct.
