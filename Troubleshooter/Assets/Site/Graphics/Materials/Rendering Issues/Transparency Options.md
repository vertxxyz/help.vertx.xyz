## Transparency options
### Description
Transparency is a troublesome topic for realtime rendering. As [previously mentioned](Transparent%20Materials.md), transparent materials do not have the luxury of being properly sorted, and there is currently no ideal solution to resolve the sorting of transparent materials in a realtime context.

### Resolution
There are many options for resolving transparency sorting problems while maintaining a transparent look:  

- [Switching to a Cutout material](Transparent%20To%20Cutout.md). Many material situations actually do not warrant a transparent shader. Foliage (leaves, grasses, and plants) is often treated as cutout. Take a close look at other games and see if you can utilise the same technique.  
- [Use dithering](Dithered%20Materials.md). Dithered materials are cutout materials that create an effective gradient of transparency whilst never actually being transparent. This is achieved by a screen-space per-pixel mask that can be appealing, or disguised via anti-aliasing solutions.  
![Dithering](dithering.png)
- [Render Queue](Render%20Queue.md). Forcing the draw order of a material. This only solves inter-material rendering issues.
- [Writing to Depth](Depth%20Rendering.md). Forcing a material to define its depth in space, at the cost of appearing solid to transparent objects that try to render behind it afterwards.  
- If the issues are in one mesh only and not between objects, sometimes strategically separating portions of the mesh can result in a consistent viewing experience where each of the new object's centers will be correctly sorted.