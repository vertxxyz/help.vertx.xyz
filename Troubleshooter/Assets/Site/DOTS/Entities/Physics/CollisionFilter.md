## CollisionFilter

Because colliders bake their Collision Filters from the Layer Collision Matrix, and queries also use a Collision Filter, unlike normal LayerMask-based physics queries, Unity Physics queries end up relying on the matrix.

See **Edit | Project Settings | Physics | Settings**, and the **Layer Collision Matrix** at the bottom.

^^^
![Layer Collision Matrix](../../../Physics%20Messages/collision-layer-matrix.png)
^^^ See [Layer-based collision detection](https://docs.unity3d.com/Manual/LayerBasedCollision.html) for more information.

Both object's `BelongsTo` and `CollidesWith` must match to successfully interact.

### Filtering logic

^^^
:::note
> When determining if two colliders should collide or a query should be performed, Unity Physics checks the `BelongsTo` bits of one against the `CollidesWith` bits of the other. Both objects must want to collide with each other for the collision to happen.

```csharp
public static bool IsCollisionEnabled(CollisionFilter filterA, CollisionFilter filterB)
{
    if (filterA.GroupIndex > 0 && filterA.GroupIndex == filterB.GroupIndex)
    {
        return true;
    }
    if (filterA.GroupIndex < 0 && filterA.GroupIndex == filterB.GroupIndex)
    {
        return false;
    }
    return
        (filterA.BelongsTo & filterB.CollidesWith) != 0 &&
        (filterB.BelongsTo & filterA.CollidesWith) != 0;
}
```

:::
^^^ From the [Filtering](https://docs.unity3d.com/Packages/com.unity.physics@latest/index.html?subfolder=/manual/collision-queries.html#filtering) section of the Collision Queries documentation.

### Looking at the data

You can check the **Physics Collider | Collision Filter** or **Physics Collider | Geometry | Child | Collision Filter** of the objects you expect to interact with, and validate that your collision filter matches.
Note that the drawer may display 0-indexed layers, not the raw bitmask value.
