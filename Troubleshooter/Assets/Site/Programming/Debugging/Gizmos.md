### Gizmos
#### Description
Unity's [Gizmos](https://docs.unity3d.com/ScriptReference/Gizmos.html) are **pickable** shapes used for debugging and content setup in the Scene View.  
By drawing in the scene and gameview you can validate assumptions about constructs used in code.
#### Usage
Gizmos can only be drawn from two functions provided by `MonoBehaviour`, [OnDrawGizmos](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html) and [OnDrawGizmosSelected](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmosSelected.html).  
`OnDrawGizmosSelected` will only run when the GameObject containing your component is selected.  

Unlike [Draw Functions](Draw%20Functions.md), gizmo functions do not take a color parameter and instead are driven by [Gizmos.color](https://docs.unity3d.com/ScriptReference/Gizmos-color.html).  
Many functions will not take arguments for rotation, and instead [Gizmos.matrix](https://docs.unity3d.com/ScriptReference/Gizmos-matrix.html) must be used.