# UI Toolkit performance debugging

Use [`PanelSettings.SetPanelChangeReceiver`](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/UIElements.PanelSettings.SetPanelChangeReceiver.html) to receive every change event. Comparing the timing of events with captures from the [Profiler](../Debugging/Performance%20Profiling.md) you can get a good picture of what change is causing hitches.

## Common performance pitfalls
- Adding or removing a class on an element with many children.
  - In this case, prefer using inline styles where possible.
- Using nested masks.
  - Apply [`UsageHint.MaskContainer`](https://docs.unity3d.com/ScriptReference/UIElements.UsageHints.MaskContainer.html) on the root of the mask to reset batching.
- Moving an element frequently.
  - Apply [`UsageHint.DynamicTransform`](https://docs.unity3d.com/ScriptReference/UIElements.UsageHints.DynamicTransform.html) so transform calculations are performed on the GPU.
- Moving an element and its children frequently.
  - Apply [`UsageHint.GroupTransform`](https://docs.unity3d.com/ScriptReference/UIElements.UsageHints.GroupTransform.html) and [`UsageHint.DynamicTransform`](https://docs.unity3d.com/ScriptReference/UIElements.UsageHints.DynamicTransform.html) to its children.
- Using more than 7 unique textures in one UI area.
  - Use a [Sprite Atlas](https://docs.unity3d.com/Manual/class-SpriteAtlas.html) to group your textures as one.
  - Configure the [Dynamic Atlas Settings](https://docs.unity3d.com/Manual/UIE-Runtime-Panel-Settings.html) on the Panel Settings asset to better encompass your textures.
- Abusing frequently-called events like `GeometryChangedEvent`.

### See also
- [Getting the best performance with UI Toolkit | Unite 2024](https://www.youtube.com/watch?v=bECmaYIvZJg)
