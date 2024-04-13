# [Quaternion.Euler](https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html)
## Declaration
```csharp
public static Quaternion Euler(float x, float y, float z);
public static Quaternion Euler(Vector3 euler);
```

## Description
When working with rotations about the X, Y, and Z axes you may find it more convenient to use Euler angles instead of [`AngleAxis`](AngleAxis.md).  

```csharp
// 30 degrees rotation around Y.
transform.rotation = Quaternion.Euler(0, 30, 0);
```

## In use
Setting [`eulerAngles`](https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html) or [`localEulerAngles`](https://docs.unity3d.com/ScriptReference/Transform-localEulerAngles.html) on a Transform will call this function to create the Quaternion used for rotation:

<<Code/Info/Quaternions/EulerAngles.html>>  

---
Return to [Quaternions.](../Quaternions.md)  
Next, [Quaternion.AngleAxis.](AngleAxis.md)
