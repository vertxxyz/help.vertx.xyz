# Line Renderer: Corner vertices

At acute angles line width cannot be maintained when the amount of vertices that makes up the corner is 0, because each segment shares the same vertices as the previous.

## Resolution
Increase the corner vertices property to give enough geometry to allow for full-width lines.
