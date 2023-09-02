## Physics queries (2D): Incorrect parameters
A `LayerMask` can accidentally be passed as an incorrect parameter of query, and as `LayerMask` is [implicitly convertable to `int`](https://github.com/Unity-Technologies/UnityCsReference/blob/e7d9de5f09767c3320b6dab51bc2c2dc90447786/Runtime/Export/Scripting/LayerMask.bindings.cs#L21), and `int` is to `float`, this mistake will not create a compiler error.

::::note
#### 🔴 Incorrect
```csharp
var hit = Physics2D.Raycast(origin, direction, layerMask);
```

:::error{.small}
The 3rd parameter for this overload is `distance`, not a mask.
:::  
::::

### Resolution
Check the parameters used in overloads of your query using [the documentation](https://docs.unity3d.com/ScriptReference/Physics2D.html), your usage must match the method signature.
:::note
#### 🟢 Correct
```csharp
var hit = Physics2D.Raycast(origin, direction, distance, layerMask);
```
:::  
:::error  
Parameters should be listed as you type, making this mistake should be difficult.  
If this is an issue you must [configure your IDE](../IDE%20Configuration.md) to get proper intellisense support.  
:::

#### Example signatures
Note that in all of these methods, `maxDistance` comes before `layerMask`.
```csharp
public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
```

---

[I am still having problems with my query.](Physics%20Queries%202D.md)