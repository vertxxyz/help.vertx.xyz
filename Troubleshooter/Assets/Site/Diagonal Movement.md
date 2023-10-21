## Diagonal movement and normalization
If digital input is not [normalized](https://docs.unity3d.com/ScriptReference/Vector3-normalized.html) before it's used, it will be a different magnitude (length) when applied at a diagonal.

Picture the vector that makes up movement as a square, the magnitude of a vector at one of the corners of the square is $\sqrt{2}$ (`1.4142...`)[^1], it should be `1` instead, which is achieved by normalization.


### Interactive diagram
<script type="module" src="/Scripts/Interactive/Vectors/normalisation.js?v=1.0.0"></script>

:::note{.center}
Press the button below the diagram to toggle between **unscaled** and **normalized** values.
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

::::note
#### I want continuous `0->1` movement maintained from analog controls like joysticks
Normalisation is not always desirable. If your input is analog—meaning it has continuous input values, not just `0` and `1`—then you may just want to ensure it's within a `0->1` range by using [`ClampMagnitude`](https://docs.unity3d.com/ScriptReference/Vector3.ClampMagnitude.html).

```csharp
Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

// 🔴 Incorrect.
Vector3 movement = input * speed;

// 🟢 Correct, input is clamped to a max length of 1 before being scaled.
Vector3 movement = Vector3.ClampMagnitude(input, 1) * speed;
```
::::

::::note
#### I want normalized movement (`0` or `1`) regardless of the controls
For some games with strict controls it can be desirable to have digital movement. In this case you should [normalize](https://docs.unity3d.com/ScriptReference/Vector3-normalized.html) your input vector before using it.

```csharp
Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

// 🔴 Incorrect.
Vector3 movement = input * speed;

// 🟢 Correct, input is normalized before being scaled.
Vector3 movement = input.normalized * speed;
```

::::

:::info
If you scale multiple `float` arguments with a vector you should multiply them together first with brackets: `vector * (scalarA * scalarB)`.  
This avoids extra multiplications.
:::

[^1]: The magnitude of a 2D vector is $\sqrt{x^2 + y^2}$. This is just the [Pythagorean theorem](https://en.wikipedia.org/wiki/Pythagorean_theorem) applied to the triangle constructed by the two axes that are at a right angle to each other.