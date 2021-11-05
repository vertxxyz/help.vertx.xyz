## Shader Graph: Alpha
### Description
The preview for the Sample Texture 2D node does not show the alpha channel from an image.  
All content (even that that is usually displayed as transparent) in the RGB channels is displayed.  
To reduce fringing artifacts at the transitions around transparent parts of an image, colour is usually flood-filled into the surrounding areas by the application that created the original image, and this is what you are seeing.

### Resolution
Ignore the RGB representation of the image, and feed the Alpha (A) channel from the sampler into the Alpha output of the graph.  

If you do want to see a version of the RGB where transparency is black, multiply the RGB channels with the A channel. It is best not to use this as the output as it may introduce transparency artifacts.  

