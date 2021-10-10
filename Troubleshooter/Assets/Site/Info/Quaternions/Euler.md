## [Quaternion.Euler](https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html)
### Declaration
```csharp
public static Quaternion Euler(float x, float y, float z);
public static Quaternion Euler(Vector3 euler);
```

### Description
When working with rotations about the X, Y, and Z axes you may find it more convenient to use Euler angles instead of [AngleAxis](AngleAxis.md).  


When setting Transform's [eulerAngles](https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html) and [localEulerAngles](https://docs.unity3d.com/ScriptReference/Transform-localEulerAngles.html) properties they are both using this function to assign to the internal Quaternion:

<<Code/Info/Quaternions/EulerAngles.html>>  

