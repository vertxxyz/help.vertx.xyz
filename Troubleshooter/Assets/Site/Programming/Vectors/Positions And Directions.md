## Positions and directions
A common misuse of vectors involves using a position where a direction is required.

Picture a map where treasure is buried on an island.

<script type="module" src="/Scripts/Interactive/Vectors/vectors.js?v=1.0.0"></script>

<canvas id="vectors-map" width="500" height="500"></canvas>

### Frames of reference
#### World/global space
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
The world space position of our X is relative to the map.

<canvas id="vectors-map__x--global" width="500" height="500"></canvas>

The local space position of our X is relative to a chosen object.

<canvas id="vectors-map__x--local" width="500" height="500"></canvas>

Note how both positions are vectors, vectors that have no meaning without a frame of reference.  
If you're given the value of a position you can't know where it is without knowing what space it was provided in.

### Directions
Think of directions as an arrow, a direction can have a magnitude (a length), or be normalised.  
A normalised direction has a magnitude of 1, and can be simply multiplied with a value to be scaled to that length.  

A position is just a direction from the origin of its frame of reference.

:::::{.interactive-content}  
<canvas id="vectors-map__positions" width="500" height="500"></canvas>
:::: {.control-root}
::: {#vectors-map__positions--toggle-space-button .interactive-button}  
Global space  
:::  
::: {#vectors-map__positions--toggle-origin-button .interactive-button}  
From origin  
:::  
::::  
:::::  

```csharp
// World-space position of the treasure is the same as its direction from the origin.
Vector3 direction = treasurePosition;
```

```csharp
// Subtracting the world-space position of the boat from the treasure gives you the world-space direction.
Vector3 direction = treasurePosition - boatTransform.position;
```

```csharp
// InverseTransformDirection converts a world-space direction to a local one. 
Vector3 direction = boatTransform.InverseTransformDirection(treasurePosition);
```

🚧 Under Construction 🚧