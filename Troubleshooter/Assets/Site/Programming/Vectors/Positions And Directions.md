## Positions and directions
It's important to distinguish positions and vectors, and the various spaces they are relative to. Without understanding this you can easily mistake a position for a direction, leading to various unexplainable interactions.

Picture a map where treasure is buried on an island.

<script type="module" src="/Scripts/Interactive/Vectors/vectors.js?v=1.0.0"></script>

<canvas id="vectors-map" width="500" height="500"></canvas>

### Frames of reference
#### World space
In our example, world space can be interpreted as the entire map, centered on its origin (bottom left in our example), with up aligned north, and right aligned east.

<canvas id="vectors-map__global" width="500" height="500"></canvas>

#### Local space
If we instead center the coordinate system on our island, this is an example of a local space.  

<canvas id="vectors-map__local" width="500" height="500"></canvas>

There can be many different local spaces, it's a frame of reference aligned to anything. In Unity a [transform](https://docs.unity3d.com/Manual/class-Transform.html) is a way to define a new local space. There's a local space relative to any object on our island, any tree, bird, rock, or boat has its own local space.

::::hidden
:::{#boat-img}
![Boat](boat.svg)
:::
:::{#x-img}
![X](x.svg)
:::
::::

<canvas id="vectors-map__local--multi" width="500" height="500"></canvas>

### Positions
Think of positions as a X, as in X marks the spot.  

Positions are only useful with a frame of reference.  
#### World space
The world space position of our X is relative to the map.

<canvas id="vectors-map__x--global" width="500" height="500"></canvas>

#### Local space
The local space position of our X is relative to a chosen object.

<canvas id="vectors-map__x--local" width="500" height="500"></canvas>

Note how both positions are vectors, vectors that have no meaning without a frame of reference.  
If you're given the value of a position you can't know where it is without knowing what space it was provided in.

### Directions
Think of directions as an arrow. A direction is just a position from the origin of its frame of reference.

#### Directions towards positions
A direction towards something has no meaning without a space and an origin to be relative to. If you applied the same arrow to another position on the map you would not find the treasure.

:::::{.interactive-content}  
<canvas id="vectors-map__positions" width="500" height="500"></canvas>
:::: {.control-root}
::: {#vectors-map__positions--toggle-space-button .interactive-button}  
Global space  
:::  
::: {#vectors-map__positions--toggle-origin-button .interactive-button}  
World relative
:::  
::::  
:::::  

::: {.vectors-code__positions--origin-world_space}
```csharp
// The world space direction relative to the world is the same as its position.
Vector3 direction = treasurePosition;
```
:::  
::: {.vectors-code__positions--origin-local_space .hidden}
```csharp
// The local space direction relative to the world is the same as world space.
Vector3 direction = treasurePosition;
```
:::  
::: {.vectors-code__positions--boat-world_space .hidden}

```csharp
// The world space direction relative to a transform is the transform's world position subtracted from the target's world position.
Vector3 direction = treasurePosition - boatTransform.position;
```
:::  
::: {.vectors-code__positions--boat-local_space .hidden}
```csharp
// InverseTransformDirection converts a world space direction to a local one.
Vector3 direction = boatTransform.InverseTransformDirection(treasurePosition - boatTransform.position);
// This is the same as transforming the world space position into local space considering the transform position.
Vector3 direction = boatTransform.InverseTransformVector(treasurePosition);
```
:::

#### Relative directions
What if we found the treasure, but lost our map! Using our compass we can head north to find the edge of the island the boat is moored on.  
Note how no matter where we are on the island, north is always the same vector. It does not need an origin to be relative to.

🚧 Diagram under construction 🚧

<canvas id="vectors-map__relative" width="500" height="500"></canvas>

#### Normalisation
A direction can have a magnitude (a length), or be normalised. A normalised direction has a magnitude of 1, and can be multiplied with a value to be scaled to that length.  
This is very helpful, because we can scale the vector based on various things to move at a certain speed, or to place something a specific distance away. Without normalising before scaling the resulting movement or position would vary wildly based on the magnitude of the original vector.

---

🚧 This page is currently under construction 🚧

See [draw functions](../Debugging/Draw%20Functions.md) to learn how to visualise these vectors in Unity.