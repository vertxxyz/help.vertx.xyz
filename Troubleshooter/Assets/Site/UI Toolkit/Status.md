# The state of UI Toolkit
This page functions as an overview of lacking features encountered while working with UI Toolkit in Unity 6.  

Last edited 2024/12/26.

## Styling
- No custom shader support.
  - Hack in your own custom shader, but note that this will break batching.
  - Use render textures (e.g. CustomRenderTexture), though note more than 6 unique (un-atlased) textures in a hierarchy breaks batching.
- No repeating 9-slice support.
  - Make your own using nested elements or mesh generation.
- No glow/drop shadow support.
  - Hack in your own with an extra element or binding and mesh generation.
  - Use custom textures and nested elements.

## Text
- Rich text tags (e.g. `<color>` and `<link>`) can't be targeted by USS, and therefore can't respond to dynamic styling.
- No max visible characters support.
  - Manually perform string building.
- No support for dynamic text gradients.
  - Hack in your own support to populate the static and internal Color Gradient asset lookup.

🚧 Under Construction 🚧
