## Positions and directions
A common misuse of vectors involves using a position where a direction is required.

Picture a map with treasure buried on an island.

<script type="module" src="/Scripts/Interactive/Vectors/vectors.js?v=1.0.0"></script>

<canvas id="vectors-map" width="500" height="500"></canvas>

### Frames of reference
#### Global space
In our example, global space can be interpreted as the entire map, centered on its origin (bottom left in our example), with up aligned north, and right aligned east.

<canvas id="vectors-map__global" width="500" height="500"></canvas>

#### Local space
If we instead center the coordinate system on our island, this is an example of a local space.  

<canvas id="vectors-map__local" width="500" height="500"></canvas>

There can be many different local spaces, it's a frame of reference aligned to anything. In Unity a Transform is a way to define a new local space. There's a local space relative to any object on our island, any tree, any bird, or rock has its own local space.

<canvas id="vectors-map__local--multi" width="500" height="500"></canvas>

### Positions
Think of positions as a X, as in X marks the spot.  


Positions are only useful with a frame of reference.  
The global space position of our X is relative to the map.

The local space position of our X is relative to a single object on our map.

Note how both are vectors that have no meaning without a frame of reference. If you're given a position you can't know what that is without knowing what space it was given in.

### Directions
Think of directions as an arrow, a direction can have a magnitude (a length), or be normalised.  
A normalised direction has a magnitude of 1, and can be simply multiplied with a value to be scaled to that length.  

A position is just a direction from the origin of its frame of reference.