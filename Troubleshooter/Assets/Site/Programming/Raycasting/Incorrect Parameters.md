## Incorrect parameters
### Description
Occasionally I see a `LayerMask` passed as an incorrect parameter of `Raycast`.  
For example, if your function call looks like this:  
```csharp
if (Physics.Raycast(origin, direction, out hit, layerMask))
{
    ...
```
This is **incorrect**. The 4th parameter for this overload is `maxDistance`, not a mask.  
As LayerMask is implicitly convertable to int, and int is to float, this mistake will not create a compiler error.  

### Resolution
Ensure that the correct parameters are used.  
The parameters for the functions are listed in [the documentation](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html).  
Eg.  
```csharp
public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);
```

:::info
Parameters should be listed as you type, making this mistake should be difficult.  
If this is an issue you should [configure your IDE](../IDE%20Configuration.md) to get proper intellisense support.  
:::  

---

[I am still having problems with my raycast.](Visual%20Debugging.md)