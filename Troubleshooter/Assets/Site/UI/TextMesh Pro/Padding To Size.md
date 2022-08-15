## TextMesh Pro: Padding to sampling point size
### Description
If there isn't enough padding surrounding text then features will blur together when viewed at a distance due to the nature of TextMesh Pro's signed distance field font representation.

### Resolution
Increase the **padding** on the Font Asset through the Update Atlas Texture button or the Font Atlas Creator.  

Typically the ratio of **padding** to **sampling point size** should be **1:10**. For example, a sampling point size of `90` would have a padding of `9`. Increase the padding as required. A larger **atlas resolution** may be needed to accomodate these changes.  

:::info{.inline}
This ratio also affects the range of material properties like Outline and Underlay. Increasing the ratio will increase their size.
:::

:::warning{.inline}
Do not increase the Atlas Resolution beyond 2048✕2048 when targeting mobile devices.
:::