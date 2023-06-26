## Incorrect parameters

A `LayerMask` can accidentally be passed as an incorrect parameter of `Raycast`.  

#### 🟢 Correct
```csharp
if (Physics.Raycast(origin, direction, out hit, distance, layerMask))
```

#### 🔴 Incorrect
```csharp
if (Physics.Raycast(origin, direction, out hit, layerMask))
```
:::warning{.inline}
The 4th parameter for this overload is `maxDistance`, not a mask.
:::  
As `LayerMask` is implicitly convertable to `int`, and `int` is to `float`, this mistake will not create a compiler error.  

### Resolution
Check the parameters used in overloads of `Raycast` using [the documentation](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html), your usage must match the method signature.  
#### Example signature  
```csharp
public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);
```

:::info
Parameters should be listed as you type, making this mistake should be difficult.  
If this is an issue you should [configure your IDE](../IDE%20Configuration.md) to get proper intellisense support.  
:::  

---

[I am still having problems with my raycast.](Incorrect%20Methods.md)