## Diagonal movement and normalization
If digital input is not [normalized](https://docs.unity3d.com/ScriptReference/Vector3-normalized.html) before it's used, it will be a different magnitude (length) when applied at a diagonal.

Picture the vector that makes up movement as a square, the magnitude of a vector at one of the corners of the square is $\sqrt{2}$ (`1.4142...`), it should be `1` instead, which is achieved by normalization.


### Interactive diagram
<script type="module" src="/Scripts/Interactive/Vectors/normalisation.js?v=1.0.0"></script>

:::note{.center}
Press the button to toggle between **unscaled** and **normalized** values.
:::

:::::{.interactive-content}  
<canvas id="normalise" width="500" height="500"></canvas>
:::: {.control-root}
::: {#normalise--toggle-normalized-button .interactive-button}  
Unscaled
:::
::::  
:::::

### Resolution

[Normalize](https://docs.unity3d.com/ScriptReference/Vector3-normalized.html) your input vector before using it.

```csharp
Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

// 🔴 Incorrect.
Vector3 movement = input * speed;

// 🟢 Correct, input is normalized before being scaled.
Vector2 movement = input.normalized * speed;
```

:::info
If you scale multiple `float` arguments with a vector you should multiply them together first with brackets: `vector * (scalarA * scalarB)`.  
This avoids extra multiplications.
:::