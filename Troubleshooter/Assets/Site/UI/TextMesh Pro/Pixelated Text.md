## TextMesh Pro: Pixelated text
### Resolution
1. Check the [Game view scale](../../Interface/Game%20View/Game%20View%20Zoom.md) is set to **1x**.
2. Switch the material used by the font asset to use one of the TextMesh Pro **Distance Field** shaders. The **Atlas Render Mode** used by the Font Atlas should typically be set to the default SDFAA to support varied font sizes[^1].
3. Set the local scale of all UI transforms below the canvas root to `(1, 1, 1)`. Instead use rect transform size and anchors to correctly scale UI components. The canvas scaler should dynamically scale the canvas while the rect transform positions and resizes itself based on those adjustments.

[^1]: Smooth or Raster rendering will only render correctly at the baked **Sampling Point Size** and requires a Bitmap shader.