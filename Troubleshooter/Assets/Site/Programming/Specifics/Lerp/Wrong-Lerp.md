## Wrong-lerp
### Description
Lerp is **linear**, if `t` changes at a constant speed, so does the output.  
Wrong-lerp is an application of lerp that produces smooth, yet imperfect movement towards a target value.  
Accompanied by such phrases like:
> Your using Lerp wrong.  

This common hacky application easily creates smooth motion.  

```csharp
value = Mathf.Lerp(value, target, Time.deltaTime * speed);
```

### Downsides
- `speed` isn't speed, it's vague quickness.
- `target` is approached, it isn't reached. We move a vague proportion towards it instead.
- The result depends on the frame rate.  
    Although it's common to see `deltaTime` used, its function is only vaguely associated with modifying the result.  

*Vague* is the takeaway from this usage. If you don't need exact outcomes or durations and aren't too worried about differences across frame rates, applying lerp like this is a common creative way to smooth movement.  

### Conclusion

If you are concerned about any of the downsides, consider alternatives like:
- [Using Lerp in a Coroutine](Coroutines.md), or applying similar logic in Update.
- Using a tweening library.
- Using [SmoothDamp](https://docs.unity3d.com/ScriptReference/Mathf.SmoothDamp.html).
- Using [MoveTowards](https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html).