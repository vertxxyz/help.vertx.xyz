## Transparency fringing
### Description
Values right outside the border of transparent regions may be saved as white, with image authoring applications varying on their handling of regions in full transparency.
With different resolutions and padding, this area of unwanted values can bleed into the edges of transparency, causing fringing artifacts.

### Resolution
If you're importing a **PSD**, try enabling **Remove Matte** in the [texture import settings](https://docs.unity3d.com/Manual/class-TextureImporter.html) for the source asset in the [Project window](https://docs.unity3d.com/Manual/ProjectView.html).

Playing with the **Mip Maps Preserve Coverage** settings may improve the fringing when viewing textures smaller than their authored resolution.
 
Otherwise, when creating your texture, you need to fill the RGB channels fully with color. I've used [Flaming Pear - Solidify](http://www.flamingpear.com/freebies.html) when using Photoshop to perform that fill. You will need to explicitly create an Alpha channel to handle the export properly.  
Not only that, but the texture format you save must export those RGB channels properly. A free plugin like [SuperPNG](http://www.fnordware.com/superpng/) handles this well for PNG; TGA and TIFF historically worked in Photoshop without issues.