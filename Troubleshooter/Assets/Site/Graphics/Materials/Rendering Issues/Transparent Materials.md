## Transparent materials


Rendering in game engines often involves two passes, an opaque, and a transparent pass.  
Opaque objects are rendered to the [depth buffer](https://en.wikipedia.org/wiki/Z-buffering), which is used to accurately determine what object is on top of another as rendering progresses.  
The transparent pass often does not have this luxury, as if they were to write to this buffer they would block out other objects that should be seen through them.  

Instead, transparent objects are sorted by their object center and rendered without further sorting. This can cause some objects to appear in front of others when their actual meshes are behind each other.  

Within a single mesh the triangles are rendered one by one, and are not sorted by depth. This can cause issues of overlapping geometry within a single object when viewed from some directions.  

---

- [My object is already an opaque material.](Opaque%20Materials.md)
- [My object should be opaque/solid](Transparent%20To%20Opaque.md)—no transparency.
- [My object should appear partially cutout](Transparent%20To%20Cutout.md)—with solid and see-through parts.
- [My object should be transparent.](Transparency%20Options.md)