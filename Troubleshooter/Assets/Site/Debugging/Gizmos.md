## How to use Gizmos

[Gizmos](https://docs.unity3d.com/ScriptReference/Gizmos.html) are shapes associated with components, used for debugging and content setup in the Scene view.  
By drawing in the Scene and Game view you can validate assumptions about your code.

### Usage
Gizmos can only be drawn from two message functions provided by `MonoBehaviour`, [`OnDrawGizmos`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html) and [`OnDrawGizmosSelected`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmosSelected.html).
`OnDrawGizmosSelected` will run when the GameObject containing your component is selected.

You can also draw gizmos from a static callback decorated with the [`DrawGizmo`](https://docs.unity3d.com/ScriptReference/DrawGizmo.html) attribute. This separates gizmo-drawing code from the runtime components and builds.

Gizmos are drawn for a single frame, and must be invoked continuously in the chosen callback function.

#### Color and rotation
Unlike [draw functions](Draw%20Functions.md), gizmo functions don't take a color parameter and instead are driven by [`Gizmos.color`](https://docs.unity3d.com/ScriptReference/Gizmos-color.html).
Most functions don't take arguments for rotation, and instead [`Gizmos.matrix`](https://docs.unity3d.com/ScriptReference/Gizmos-matrix.html) must be used.

### Visibility

To display gizmos in the Scene or Game view, toggles for each must be enabled.

![Scene view gizmo toggle](../Scene%20View/scene-view-gizmo-toggle.png)

If your gizmo still isn't appearing, [follow these steps](../Gizmos/Enabling%20Gizmos.md).

---
[Return to visual debugging.](Visual%20Debugging.md)
