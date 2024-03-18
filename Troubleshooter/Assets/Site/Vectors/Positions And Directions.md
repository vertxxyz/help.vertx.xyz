## Positions and directions
It's important to distinguish between **positions** and **directions**, and the various spaces they are relative to.  
Common mistakes relating to these concepts include:
- Using a position where a direction is asked for, resulting in large incorrect movements further from the origin.
- Using a direction where a position is asked for, resulting in movements stuck near the origin.
- Applying a world or local space vector in the opposing space, resulting in an incorrect transformation.

So let's learn how to use vectors, and how to avoid using them incorrectly.

### Vectors
First, understand that a vector is just a series of numbers.
```csharp
// (0, 100)
var a = new Vector2(0, 100);
// (0, 100, 42)
var b = new Vector3(0, 100, 42);
```
A vector has no meaning by itself, and if I gave you either of these vectors you would have to make assumptions regarding their meaning before performing any useful operations with them.
Maybe these are **positions** in space, but maybe they're **directions** to move in, **rotations**, **colours**, or perhaps **packed information** ready to pass to an Animator or the GPU? Consider where you got the vector from, and what transformations have been performed on it to inform its current meaning.

<script type="module" src="/Scripts/Interactive/Vectors/vectors.js?v=1.0.0"></script>

### Frames of reference
Picture a map where treasure is buried on an island.

#### World space
In our example, world space can be interpreted as the entire map, centered on its origin (bottom left in our example), with up aligned north, and right aligned east.

<canvas id="vectors-map__global" width="500" height="500"></canvas>

#### Local space
If we instead center the coordinate system on our island, this is an example of a local space.

<canvas id="vectors-map__local" width="500" height="500"></canvas>

There can be many different local spaces, it's a frame of reference aligned to anything. In Unity a [transform](https://docs.unity3d.com/Manual/class-Transform.html) is a way to define a new local space. There's a local space relative to any object on our island; any tree, bird, rock, or boat has its own local space.

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
Think of positions as an **X**, as in **X** marks the spot.

Positions are only useful with a frame of reference.
#### World space
The world space position of our **X** is relative to the map.

<canvas id="vectors-map__x--global" width="500" height="500"></canvas>

#### Local space
The local space position of our **X** is relative to a chosen object.

<canvas id="vectors-map__x--local" width="500" height="500"></canvas>

Take note that while the position appears to be the same, the vectors are different. All vectors have no meaning without a frame of reference.
If you're given a position you can't know where it is without knowing the space it was provided in.

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

::: {.vectors-code__positions--origin-world_space .note}
If someone else had a map without an **X** drawn on it, you would provide them with the world space position so they could annotate their map.
```csharp
// The world space direction relative to the world is the same as its position.
Vector3 direction = map.TreasurePosition;
```
:::
::: {.vectors-code__positions--origin-local_space .hidden .note}
If someone else had a map without an **X** drawn on it, you would provide them with the world space position so they could annotate their map.
```csharp
// The local space direction relative to the world is the same as in world space.
Vector3 direction = map.TreasurePosition;
```
:::
::: {.vectors-code__positions--boat-world_space .hidden .note}
If we knew where we were on the map, we could calculate a world space direction to travel in relative to our boat.  
By using a compass and staying on that direction over its distance, we could reach the **X**.  
```csharp
// The world space direction relative to a transform
// is the transform's world position subtracted from the target's world position.
Vector3 direction = map.TreasurePosition - boatTransform.position;
```
:::
::: {.vectors-code__positions--boat-local_space .hidden .note}
If we wanted to know how to move the boat to get closer to the **X**, we would need to know what coordinates to input into its controls. We would need a local space direction, relative to the boat.
```csharp
// InverseTransformDirection converts a world space direction to a local one.
Vector3 direction = boatTransform.InverseTransformDirection(map.TreasurePosition - boatTransform.position);

// This is the same as transforming the world space position into local space while considering the transform position.
Vector3 direction = boatTransform.InverseTransformVector(map.TreasurePosition);
```
:::

#### Relative directions
What if we found the treasure, but lost our map! Using our compass we can head north to find the edge of the island the boat is moored on.
Note how no matter where we are on the island, north is always the same vector. It does not need an origin to be relative to.

🚧 Diagram under construction 🚧

[//]: # (<canvas id="vectors-map__relative" width="500" height="500"></canvas>)





#### Normalisation
A direction can have a magnitude (a length), or be normalised. A normalised direction has a magnitude of 1, and can be multiplied with a value to be scaled to that length.
This is very helpful, because we can scale the vector based on various things to move at a certain speed, or to place something a specific distance away.
Without normalisation, the length of the scaled vector would vary wildly based on the magnitude of the original.

🚧 Diagram under construction 🚧

---

🚧 This page is currently under construction 🚧

---

See [Vectors](https://docs.unity3d.com/Manual/VectorCookbook.html) in the Unity Manual to learn more.  
See [visual debugging](../Debugging/Visual%20Debugging.md) to learn how to visualise these vectors in Unity.  
