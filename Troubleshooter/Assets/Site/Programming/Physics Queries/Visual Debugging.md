## Physics queries: Visual debugging
To debug the location of rays use [`Debug.DrawRay`](https://docs.unity3d.com/ScriptReference/Debug.DrawRay.html).

```csharp
Debug.DrawRay(ray.origin, ray.direction * distance);
```

This can help validate assumptions about spaces, and what the ray is reaching.  
If a ray is colliding unexpectedly it is worth using [`Debug.DrawLine`](https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html) to draw between the origin and hit point.

```csharp
Debug.DrawLine(ray.origin, hit.point);
```

You can apply similar techniques to understand the relationships between any query and the scene. Make sure you have understood the difference between DrawRay and DrawLine and don't mix the two up, as an incorrect visualisation can be very misleading.

---

See [draw functions](../Debugging/Draw%20Functions.md) for more information and extended options when doing complex visual debugging.

---

[I am still having problems with my query.](Errors.md)