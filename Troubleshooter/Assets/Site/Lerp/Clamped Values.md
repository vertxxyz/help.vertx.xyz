# Lerp: Clamped values
Unity's [`Lerp`](https://docs.unity3d.com/ScriptReference/Mathf.Lerp.html) clamps the input `t` value between `0` and `1`, and does not allow for overshoots.

## Resolution

Understand [Lerp](Overview.md) before using it. Lerp is commonly abused in the form of "[wrong-lerp](Wrong-Lerp.md)", which when misunderstood can easily result in mistakes involving incorrect `t` values.

If you do understand Lerp, and are just looking for an unclamped version see [`LerpUnclamped`](https://docs.unity3d.com/ScriptReference/Mathf.LerpUnclamped.html).
