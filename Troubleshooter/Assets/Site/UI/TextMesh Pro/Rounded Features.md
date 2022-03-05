## TextMesh Pro: Rounded features
### Description
TextMesh Pro typically uses signed distance fields to cheaply represent fonts well at multiple distances.  
Distance fields work best with smooth features due to their typically low resolution.

### Resolution
Increase the **Atlas Resolution** or **Sampling Point Size** until the quality of the text has reached a threshold you are happy with.  

:::warning{.inline}
Do not increase the Atlas Resolution beyond 2048✕2048 when targeting mobile devices.
:::

:::warning{.inline}
When manually increasing Sampling Point Size large values can cause characters to leave the atlas.
:::

Using a different **Render Mode** (that isn't Smooth or Raster) may also produce better results in specific circumstances. SDFAA is the default mode.