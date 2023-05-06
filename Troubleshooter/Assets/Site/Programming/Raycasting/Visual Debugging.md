## Visual debugging
To debug the location of rays use [`Debug.DrawRay`](https://docs.unity3d.com/ScriptReference/Debug.DrawRay.html).  

```csharp
Debug.DrawRay(ray.origin, ray.direction * distance);
```

This can help validate assumptions about spaces, and what the ray is reaching.  
If a ray is colliding unexpectedly it is worth using [`Debug.DrawLine`](https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html) to draw between the origin and hit point.

```csharp
Debug.DrawLine(ray.origin, hit.point);
```

---

See [Draw Functions](../Debugging/Draw%20Functions.md) for more information.