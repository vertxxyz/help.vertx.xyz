## Wrong-lerp

Lerp is **linear**, if `t` changes at a constant speed, so does the output.  
Wrong-lerp is an application of lerp that produces smooth, yet imperfect movement towards a target value.  

```csharp
value = Mathf.Lerp(value, target, Time.deltaTime * speed);
```

Accompanied by such phrases like:
> 🤓 *"Your using Lerp wrong."*  

This common hacky application easily creates smooth motion.

### Downsides
- `speed` isn't speed, it's a vague quickness.
- `target` is approached, it isn't reached. This moves a vague proportion towards it instead.
- The result depends on the frame rate.  
    Although it's common to see `deltaTime` used, its function here is only a vague improvement.  

*Vague* is the takeaway from this usage.  
If you don't need exact outcomes or durations and aren't too worried about differences across frame rates, applying lerp like this is a common creative way to smooth movement.  

:::note{.center}
#### Graph

```d3
graph-wrong-lerp
```

```csharp
value = Mathf.Lerp(value, 1, Time.deltaTime * 10);
```

:::

### Improvement

Using a more complex `t` can solve frame rate dependency problems.

```csharp
value = Mathf.Lerp(value, target, 1 - Mathf.Pow(fraction, Time.deltaTime));
```

Where `fraction` is a `0->1` factor that defines a percentage of smoothing. `0` gets you the target (no smoothing), `1` is the source (so smoothed it's useless).  

:::note{.center}
#### Graph

```d3
graph-improved-wrong-lerp
```

```csharp
fraction = 0.3;
value = Mathf.Lerp(value, target, 1 - Mathf.Pow(fraction, Time.deltaTime * 10));
```
:::

### Conclusion

If you are concerned about any of the downsides, consider alternatives like:
- [Using Lerp in a Coroutine](Coroutines.md), or applying similar logic in Update.
- Using a tweening library.
- Using [SmoothDamp](https://docs.unity3d.com/ScriptReference/Mathf.SmoothDamp.html).
- Using [MoveTowards](https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html).

Certain libraries will have their own tweening libraries built-in. For example, UI Toolkit has [USS transitions](https://docs.unity3d.com/Manual/UIE-Transitions.html).

---  
[Return to overview.](Overview.md)