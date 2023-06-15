## Linear interpolation
### Description
Linear interpolation or "[`Lerp`](https://docs.unity3d.com/ScriptReference/Mathf.Lerp.html)", is a simple function that returns a straight-line value between two points (`a` and `b`) based on a normalised value (`t`).  

```csharp
public static float Lerp(float a, float b, float t)
{
    return a + (b - a) * Clamp01(t);
}
```

### "Huh?"
**It's simple!**

- When `t` is `<= 0`, `Lerp` will return `a`.
- When `t` is `>= 1`, `Lerp` will return `b`.
- When `t` is in between `0` and `1`, `Lerp` will return a value between the two.

**"Give me an example..."**  
<<Code/Specific/Lerp/Example.html>>  

- `t = 0.0f`, `value` is `50`.
- `t = 1.0f`, `value` is `100`.
- `t = 0.2f`, `value` is `60`.
- `t = 0.5f`, `value` is `75`.
- `t = 0.8f`, `value` is `90`.

### Vector2, Vector3, Color, Quaternion...

All lerp functions are linear, and return the in-between values based on a `0->1` `t` value.  
Their usage is the same, even though the applications are varied.

### Interactive diagram
:::note{.center}
Drag the slider to modify `t`, edit `a` and `b` in the code below.
:::

<script type="module" src="/Scripts/Interactive/Lerp/scene.js?v=1.0.0"></script>  
<canvas id="lerp" width="500" height="500"></canvas>
:::slider {#lerp_time_slider .color-angle}
:::
<<Code/Specific/Lerp/Lerp.html>>  

### Usage
- [Coroutines](Coroutines.md)
- [Wrong-lerp](Wrong-Lerp.md)