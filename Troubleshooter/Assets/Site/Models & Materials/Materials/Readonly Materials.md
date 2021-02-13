### Readonly Materials

#### Description:
Materials can be readonly when imported from models or provided in readonly sources like packages.  
A model's material is a generated asset created by the importer, and cannot be modified.

#### Resolution:
Materials imported from models either must be extracted, or remapped on the model's importer.  
Select the model and head to the materials tab.
From there you can choose to either extract the materials, which will place them in a folder and remap them in the importer;
or you can remap the materials manually to others in your project.  
See the [materials tab](https://docs.unity3d.com/Manual/FBXImporter-Materials.html) documentation for more information.

---
Alternatively you can replace the material used on a prefab that is not a model prefab (a prefab directly generated from a model), or an instance, by dragging a different material into the materials slots on the [Renderer](https://docs.unity3d.com/Manual/class-MeshRenderer.html#materials) component.