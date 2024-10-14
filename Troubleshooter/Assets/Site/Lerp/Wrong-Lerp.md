---
title: "Wrong-lerp"
description: "Applying lerp so that it produces smooth, imperfect movement towards a target value."
---
# Wrong-lerp

[Lerp](Overview.md) is **linear**, if `t` changes at a constant speed, so does the output.  
Wrong-lerp is an application of lerp that produces smooth, yet imperfect movement towards a target value.  

```csharp
value = Mathf.Lerp(value, target, Time.deltaTime * speed);
```

Accompanied by such phrases like:
> 🤓 *"Your using Lerp wrong."*  

This common hacky application easily creates smooth motion.

## Downsides
- `speed` isn't speed, it's a vague quickness.
- `target` is approached, it isn't reached. This moves a vague proportion towards it instead.
- The result depends on the frame rate.  
    Although it's common to see `deltaTime` used, its function here is only a vague improvement.  

*Vague* is the takeaway from this usage.  
If you don't need exact outcomes or durations and aren't too worried about differences across frame rates, applying lerp like this is a common creative way to smooth movement.  

:::note{.center}
### Graph

```d3
graph-wrong-lerp
```

```csharp
// speed: 10
value = Mathf.Lerp(value, 1, Time.deltaTime * speed);
```

:::

## Improvement

Using a more complex `t` can solve frame rate dependency problems.

^^^
```csharp
static float ExponentialDecay(float value, float target, float decay, float deltaTime)
    => Mathf.Lerp(value, target, Mathf.Exp(-decay * deltaTime));
```
^^^ Where `decay` is `0->∞`. `0` is constant, and larger values approach the target faster.

:::note{.center}
### Graph
```d3
graph-improved-wrong-lerp
```

```csharp
// decay: 10
value = ExponentialDecay(value, target, decay, Time.deltaTime);
```
:::

### See also: Fractional approach{.foldout}
^^^
```csharp
// Using a "fraction" or "remainder" as input.
static float FractionalDamping(float value, float target, float fraction, float deltaTime)
    => Mathf.Lerp(value, target, 1 - Mathf.Pow(fraction, deltaTime));
```
^^^ Where `fraction` is a `0->1` smoothing factor.<br/>`0` gets you the target (no smoothing), `1` is the source (so smoothed it's pointless).

Using `Pow` is a more expensive approach, so consider your use cases.

## Conclusion

If you are concerned about any of the downsides, consider alternatives like:
- Using a tweening library*.
- [Using Lerp in a Coroutine](Coroutines.md), or applying similar logic in Update.
- Using [SmoothDamp](https://docs.unity3d.com/ScriptReference/Mathf.SmoothDamp.html).
- Using [MoveTowards](https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html).

*Certain libraries will have their own tweening libraries built-in. For example, UI Toolkit has [USS transitions](https://docs.unity3d.com/Manual/UIE-Transitions.html).

## Other resources

- **Rory Driscoll:** [Frame rate independent damping using Lerp](https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/)
- **Freya Holmér:** [Lerp smoothing is broken - a journey of decay and delta time](https://www.youtube.com/watch?v=LSNQuFEDOyQ)
---  
[Return to overview.](Overview.md)
