# The state of UI Toolkit
This page functions as an overview of lacking features encountered while working with UI Toolkit in Unity 6.  

Last edited 2025/05/18.

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
- Max visible characters support can be manually implemented as of 6000.2[^1].
- No support for dynamic text gradients.
  - Hack in your own support to populate the static and internal Color Gradient asset lookup.
  - Or use custom text animation[^1].
- No percentage font sized text.
  - Auto-size support is present in the [Advanced Text Generator](https://docs.unity3d.com/Manual/UIE-advanced-text-generator.html) as of **6000.2.0b4**.
  - Alternatively, calculate and set font size in code.

## Vector images
- No anti-aliasing of VectorImage.

🚧 Under Construction 🚧

[^1]: [Create custom text animation](https://docs.unity3d.com/6000.2/Documentation/Manual/ui-systems/create-custom-text-animation.html)
