### Inverted Normals

#### Description
Most materials in Unity will render single-sided. This is unlike the defaults of many modelling programs.
The direction of model faces is determined by the winding order of the vertices that make up a triangle.

#### Resolution
Research how to **invert normals** in your modelling program. Additional key words to search for are **face normals** and **backface culling**.  
**Backface Culling** describes single-sided rendering, and is something you may wish to enable in your modelling program when working with models that are to be used inside Unity.  
Not all your faces may be inverted, so make sure you understand which are. If one object has partially flipped normals there may be a **recalculate normals** operation that can unify the normal direction.  

---

[My model or material still appears inverted](../Materials/Rendering%20Issues/Transparent%20Materials.md)