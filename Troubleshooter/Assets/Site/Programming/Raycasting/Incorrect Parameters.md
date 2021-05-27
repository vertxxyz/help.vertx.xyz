### Incorrect Parameters
#### Description
Occasionally I see a `LayerMask` passed into the incorrect parameter of a `Raycast` function.
For example, if your function call looks like this:
```csharp
if (Physics.Raycast(origin, direction, out hit, layerMask)) {
```
Then this is **incorrect**, as the 4th parameter for this overload is `maxDistance`, and not a layer mask. Seeing as LayerMasks are implicitly convertable to ints, and ints are to floats, this mistake will not create a compile error.

#### Resolution
Ensure that the correct parameters are used.  
The parameters for the functions are listed in [the documentation](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html).  
Eg.  
```csharp
public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);
```

---
If this is the mistake you have made you should [configure your IDE](../IDE%20Configuration.md) to get proper intellisense support. Parameters should be listed as you type, so making this mistake should be difficult.  

[I am still having problems with my Raycast](Visual%20Debugging.md).