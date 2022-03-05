<<Abbreviations/NRE.md>>
## Unity null
Comparing [UnityEngine.Object](https://docs.unity3d.com/ScriptReference/Object.html) derived types to `null` in Unity may not work how you expect.  

### Details
The equality operators (`==` and `!=`) for `UnityEngine.Object` types have been overridden by Unity, and don't only perform reference comparisons.  
A Unity Object is split into two parts: managed C#, and native C++. Users interact with the managed object, and the engine manages the native object.  
When an Object is compared with null and it isn't *really* null, a check against the native object occurs. This makes it possible to Destroy an Object somewhere, and have an entirely different reference evaluate to null.  

The Editor adds extra context to null assignments occuring in the editor through this object.  
If something isn't assigned in the editor a custom NRE, an [Unassigned Reference Exception](../Common%20Errors/Runtime%20Exceptions/UnassignedReferenceException.md) is thrown.   
If there used to be an object, but it has been Destroyed or become invalid, you get a a  [Missing Reference Exception](../Common%20Errors/Runtime%20Exceptions/MissingReferenceException.md).  
These exceptions also come bundled with some extra context, the location that is missing the object, none of which would be provided with a normal NRE.  
Due to this [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) will allocate memory in the Editor even when nothing is found. Using [TryGetComponent](https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html) will not.  


### Ramifications
The null conditional (`?.`) and null coalescing (`??`, `??=`) operators, and `is` null checks (`is null`, `is not null`, `is {}`) will not function correctly with `UnityEngine.Object` types as these operators could not be overridden by Unity. When Unity was created these operators did not exist.  

If you want to reclaim memory on Destroyed objects you need to additionally ensure all references are set to null to allow the C# garbage collector to reclaim that memory exactly like other heap allocated types.  

---
See the 2014 blog ["Custom == operator, should we keep it?"](https://blog.unity.com/technology/custom-operator-should-we-keep-it), or the Resharper/Rider suggestion ["Possible unintended bypass of lifetime check of underlying Unity engine object"](https://github.com/JetBrains/resharper-unity/wiki/Possible-unintended-bypass-of-lifetime-check-of-underlying-Unity-engine-object) for additional details.   