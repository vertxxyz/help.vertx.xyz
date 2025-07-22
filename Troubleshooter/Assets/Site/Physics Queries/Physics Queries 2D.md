# Physics queries (2D): General
## Use the correct class
You must be using functions from the [`Physics2D`](https://docs.unity3d.com/ScriptReference/Physics2D.html) class. [`Physics`](https://docs.unity3d.com/ScriptReference/Physics.html) calls will not interact with 2D physics.

If you are calling a query like [`collider2D.Raycast(...`](https://docs.unity3d.com/ScriptReference/Collider2D.Raycast.html) from a collider, then you are using a method that can only hit that single collider.  
Make sure you understand this, or instead, use the static method [`Physics2D.Raycast`](https://docs.unity3d.com/ScriptReference/Physics2D.Raycast.html) that queries all the colliders in the scene.

## Issues with colliders
- Raycasting requires [2D colliders](https://docs.unity3d.com/Manual/Collider2D.html) to work. 3D colliders will not by hit by `Physics2D` queries.  
  You cannot query against raw meshes or sprites using `Physics2D` functions.
- Edge Colliders are single-sided. Check that you are casting against the front face.
- When using a Tile Map Collider the tile itself needs a Collision Type set.

## Correct use of ContactFilter2D
A newly constructed [`ContactFilter2D`](https://docs.unity3d.com/ScriptReference/ContactFilter2D.html) will not be correctly configured. Check that you understand how to use the API using the example below.
### Example usage
```csharp
var filter = new ContactFilter2D().NoFilter();

// Optional filtering:
filter.SetLayerMask(1 << 2); // Used to avoid having to set .useLayerMask too.
filter.SetDepth(-1, 1); // Used to avoid having to set .useDepth, comes with additional validation.
filter.SetNormalAngle(0, 180); // Used to avoid having to set .useNormalAngle, comes with additional validation.
filter.useTriggers = false; // Disables contact results based on trigger collider involvement.

// Example query
int resultCount = Physics2D.OverlapCircle(transform.position, radius, filter, results);
```

## Notes on setup
- **Casts** originating inside of colliders will often return normals that are the inverse of the ray direction.
- Don't use **casts** of `0` `maxDistance`, or `zero` `direction`, consider using **overlaps** instead.

---

[I am still having problems with my query.](NonAlloc%202D.md)
