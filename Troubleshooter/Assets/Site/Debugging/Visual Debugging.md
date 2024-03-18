## Visual debugging
Unity has many tools for debugging spacial problems, though each tool has its own benefits and drawbacks.

### General-purpose
By drawing lines and shapes in the Scene and Game view you can validate assumptions about positions and directions used in code.

#### Debug Draw
Unity's `Debug.DrawRay` and `Debug.DrawLine` are valuable tools for debugging 3D (and 2D) information. The API is simple to use, but is only capable of drawing basic lines.  
[Learn more.](Draw%20Functions.md)

#### Gizmos
MonoBehaviour's `OnDrawGizmos` and `OnDrawGizmosSelected` callbacks allow the use of [Gizmos](https://docs.unity3d.com/ScriptReference/Gizmos.html) and [Handles](https://docs.unity3d.com/ScriptReference/Handles.html) APIs. The APIs support more complex shapes, but cannot be used outside of specific callbacks.  
[Learn more.](Gizmos.md)

### Debugging physics issues
#### Physics Debugger
The Physics Debugger is a built-in tool window for visualising 3D physics shapes, collisions, and queries.  
[Learn more.](Physics%20Debugger.md)

#### Custom packages
[Vertx.Debugging](#vertx.debugging) and [Raycast Visualization](#raycast-visualization) both have drop-in replacements for physics queries (casts, overlaps, hits, checks) that display their bounds and results.

### Custom packages and assets
#### Vertx.Debugging
[Vertx.Debugging](https://github.com/vertxxyz/Vertx.Debugging) is a debugging package for drawing wireframe shapes.  
It has drop-in replacements for physics queries (casts, overlaps, hits, checks), many extra shapes, and can draw labels in the scene.  
It is designed for editor debugging and most functions will be stripped when built. Versions 3.0.0 and above support jobs.

^^^
![Vertx.Debugging](https://user-images.githubusercontent.com/21963717/194199755-a63d8ebc-0cc7-4268-9316-78f7d4fbea1a.mp4)
^^^ Visualised shapes and casts from [Vertx.Debugging](https://github.com/vertxxyz/Vertx.Debugging)

#### Raycast Visualization
[Raycast Visualization](https://github.com/nomnomab/RaycastVisualization) is a drop-in replacement for Physics queries (casts, overlaps, hits, checks).  
It is designed for editor debugging and most functions will be stripped when built.

#### ALINE
[ALINE](https://arongranberg.com/aline/) is a paid alternative that is intended to draw shapes in the editor and builds, and supports drawing from jobs.

#### Shapes
[Shapes](https://acegikmo.com/shapes/) is a paid alternative that is designed to draw good-looking lines for editor and builds.  
The API is not designed to be used inline as others

### Entities
#### Unity Physics
If you are using Entities (ECS), [Unity Physics](https://docs.unity3d.com/Packages/com.unity.physics@latest) (`com.unity.physics`) has `PhysicsDebugDisplaySystem`, which contains static helper methods for drawing wireframe shapes from jobs.  
:::warning
If you do not understand what Entities is, you should not use this package for debugging.  
:::  
There is also the [Physics Debug Display](https://docs.unity3d.com/Packages/com.unity.physics@latest/index.html?subfolder=/manual/component-debug-display.html) authoring component that has toggles for similar debugging to the [Physics Debugger](#physics-debugger).

## Comparison table

^^^

| Name                                            | 2D/3D   | Job System / Burst | Platforms       | Call site                  | Price    | Focus                        |
|-------------------------------------------------|---------|--------------------|-----------------|----------------------------|----------|------------------------------|
| Draw functions                                  | Both*   | No                 | Editor          | Anywhere†                  | Built-in |                              |
| Gizmos                                          | Both*   | No                 | Editor          | Gizmos callbacks           | Built-in |                              |
| Handles                                         | Both*   | No                 | Editor (strict) | Gizmos and OnGUI callbacks | Built-in |                              |
| Physics Debugger                                | 3D only | No                 | Editor          |                            | Built-in | Ease of use                  |
| Unity Physics                                   | Both*   | **Yes**            | Editor          | Entities                   | Built-in | Performance                  |
| [Vertx.Debugging](#vertx.debugging)             | Both    | **3.0.0+**         | Editor          | Anywhere                   | Free     | Ease of use,<br/>performance |
| [Raycast Visualization](#raycast-visualisation) | Both    | No                 | Editor          | Anywhere†                  | Free     | Ease of use                  |
| [ALINE](#aline)                                 | Both*   | **Yes**            | Any             | Anywhere                   | $30      | Performance                  |
| [Shapes](#shapes)                               | Both    | No                 | Any             | DrawShapes callback        | $100     | Looks,<br/>Ease of use       |
^^^ *No explicit 2D shapes.<br/>†No Burst support.
