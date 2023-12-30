## Scene view drag and drop
Only certain types of object support dragging into the scene.

All supported objects must be dragged from the [Project window](https://docs.unity3d.com/Manual/ProjectView.html). You cannot drag external assets into the scene without importing them into the project first.

:::note  
#### List of supported types

^^^  

| Type            | Dragging to empty space                                  | Dragging to an object                                  |
|-----------------|----------------------------------------------------------|--------------------------------------------------------|
| Models (prefab) | Creates an object in the scene.                          | -                                                      |
| Prefabs         | Creates an object in the scene.                          | -                                                      |
| Sprites         | Creates a Sprite Renderer in the scene.                  | Creates a new material and assigns it to the renderer. |
| Textures        | -                                                        | Creates a new material and assigns it to the renderer. |
| Materials       | -                                                        | Assigns it to the renderer.                            |
| Audio Clips     | Creates an object with an Audio Source.                  | Adds an Audio Source component.                        |
| Mesh assets     | Creates an object in the scene. No material is assigned. | Assigns it to the renderer.                            |
^^^ This list may not be exhaustive.

:::