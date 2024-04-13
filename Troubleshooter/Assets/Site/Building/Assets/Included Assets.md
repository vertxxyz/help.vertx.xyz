# Included assets
Assets can be stripped from a build if they aren't explicitly referenced. This reduces the size of the build, but can cause build-only issues.

## Resolution
Reference the missing asset explicitly.

### General
**Choose one**:
- Create a [serialized reference](../../References/Serialized%20References.md) from a build scene.
- Use [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@latest/) or [Asset Bundles](https://docs.unity3d.com/Manual/AssetBundlesIntro.html) to include and reference the asset.
- Add the asset to the **Preloaded Assets** section of the Player settings (**Edit | Project Settings | Player | Other Settings**).

[//]: # (- Add the asset to the Resources folder &#40;this is the least optimal way to reference an asset&#41;.)

### Shaders
- Add the asset to the **Always Included Shaders** section of the Graphics settings (**Edit | Project Settings | Graphics**).
