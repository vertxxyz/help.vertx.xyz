# Issues importing .blend files
If a `.blend` file is not recognised by Unity, it likely is not correctly associated with Blender.

## Resolution
You must have Blender installed and set as the default application for `.blend` files.

In your file explorer or operating system, associate `.blend` files to open with Blender.

## Notes
It is almost always preferred to export to a natively-supported model format like `FBX` due to the following reasons:
1. Blender is required to be installed on contributors' machines.
1. The same Blender version that authored a file can be required to import it.
1. The dependency on Blender makes it difficult to use build automation tooling.
