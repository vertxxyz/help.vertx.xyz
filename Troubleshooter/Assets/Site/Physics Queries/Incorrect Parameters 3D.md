## Physics queries (3D): Incorrect parameters
A `LayerMask` can accidentally be passed as an incorrect parameter of query, and as `LayerMask` is [implicitly convertable to `int`](https://github.com/Unity-Technologies/UnityCsReference/blob/e7d9de5f09767c3320b6dab51bc2c2dc90447786/Runtime/Export/Scripting/LayerMask.bindings.cs#L21), and `int` is to `float`, this mistake will not create a compiler error.

::::note
#### 🔴 Incorrect
```csharp
if (Physics.Raycast(origin, direction, out hit, layerMask))
```

:::error{.small}
The 4th parameter for this overload is `maxDistance`, not a mask.
:::
::::

### Resolution
Check the parameters used in overloads of your query using [the documentation](https://docs.unity3d.com/ScriptReference/Physics.html), your usage must match the method signature.
:::note
#### 🟢 Correct
```csharp
if (Physics.Raycast(origin, direction, out hit, distance, layerMask))
```
:::
:::error
Parameters should be listed as you type, making this mistake should be difficult.
If this is an issue you must [configure your IDE](../IDE%20Configuration.md) to get proper intellisense support.
:::

#### Example signatures
Note that in all of these methods, `maxDistance` comes before `layerMask`.
```csharp

public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);
public static bool Raycast(Ray ray, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation = Quaternion.identity, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation = Quaternion.identity, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool SphereCast(Ray ray, float radius, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
```

---

[I am still having problems with my query.](Physics%20Queries%203D.md)
