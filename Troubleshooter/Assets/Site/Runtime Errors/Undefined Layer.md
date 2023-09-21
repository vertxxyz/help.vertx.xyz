## Undefined Layer

```
A game object can only be in one layer. The layer needs to be in the range [0...31]
```



Individual GameObjects have layers that can be used to affect physics and rendering.  
Layers are defined in the [Tags and Layers Settings](https://docs.unity3d.com/Manual/class-TagManager.html) (**Edit | Project Settings | Tags and Layers**).  
Layers are **not** the same as Tags, and a `LayerMask` does not describe a single layer.  

If you're seeing this error you have either not defined the layer in the first place, haven't referenced an existing layer identically, or are using the wrong structure like a LayerMask.  

:::warning
**Identical** includes capitalisation and spaces
:::

### Resolution
Define an identical layer in the Tags and Layers Settings, and either directly use the index, or use [`LayerMask.NameToLayer`](https://docs.unity3d.com/ScriptReference/LayerMask.NameToLayer.html) to get the index, then assign it to the GameObject.  
A [`LayerMask`](https://docs.unity3d.com/ScriptReference/LayerMask.html) is not the same as a layer. Do not assign a mask to the `layer` property of a GameObject.

If the error still appears, ensure the console has been cleared. Then log the GameObject and its layer before you assign it to validate your logic.
See [logging: how-to](../../Debugging/Logging/Logging%20How-to.md) for more information.