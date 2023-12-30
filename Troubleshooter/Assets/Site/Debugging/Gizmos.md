## Gizmos

Unity's [gizmos](https://docs.unity3d.com/ScriptReference/Gizmos.html) are shapes associated with components, used for debugging and content setup in the Scene view.
By drawing in the Scene and Game view you can validate assumptions about constructs used in code.
### Usage
Gizmos can only be drawn from two functions provided by `MonoBehaviour`, [OnDrawGizmos](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html) and [OnDrawGizmosSelected](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmosSelected.html).
`OnDrawGizmosSelected` will run when the GameObject containing your component is selected.

Unlike [draw functions](Draw%20Functions.md), gizmo functions don't take a color parameter and instead are driven by [Gizmos.color](https://docs.unity3d.com/ScriptReference/Gizmos-color.html).
Many functions will not take arguments for rotation, and instead [Gizmos.matrix](https://docs.unity3d.com/ScriptReference/Gizmos-matrix.html) must be used.

---

To display gizmos in the Scene or Game view, toggles for each must be enabled.

![Scene view gizmo toggle](../Scene%20View/scene-view-gizmo-toggle.png)
