# Missing Shadows
## Resolution
On the **Light** component, ensure that:  
- **Mode** is set to either **Real Time** or **Mixed**.
- **Shadow Type** is set to either **Hard** or **Soft** shadows.
- **Culling Mask** includes the layers that cast and receive shadows.
- Realtime Shadows>**Strength** is set high enough to be visible.

On the **renderer** component we want to cast shadows, ensure that:
- Lighting>**Cast Shadows** is enabled.

### Built-in render pipeline
In the **Quality settings**, ensure that:
- **Shadows** is enabled, set to include the shadow type used by the light.
- **Max Shadow Distance** is set high enough. This value is the distance from the camera, not from the light.

### Scriptable render pipelines (URP, HDRP)
In the **render pipeline asset**, ensure that:
- **Lighting | Main Light/Additional Lights | Cast Shadows** are enabled depending on what type of light you're using.
- **Shadows | Max Distance** is set high enough. This value is the distance from the camera, not from the light.
