## TextMesh Pro: Blurry text
### Description
If there is not enough padding surrounding text then features will blur together when viewed at a distance due to the nature of TextMesh Pro's signed distance field font representation.

### Resolution
Increase the ratio of **Sampling Point Size** to **Padding** on the Font Asset through the Update Atlas Texture button or the Font Atlas Creator.  
Typically this ratio should be ~10%. For example, if the Sampling Point Size is `90` then you would want a padding value of `9`.  

:::info
This ratio also affects the range of material properties like Outline, Underlay, etc. Increasing the ratio will increase their size.
:::