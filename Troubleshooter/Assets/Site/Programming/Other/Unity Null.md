### Unity Null
Comparing `UnityEngine.Object` derived types to `null` in Unity may not work how you expect.  

#### Details
The equality operators (`==` and `!=`) for `UnityEngine.Object` types have been overridden by Unity, and do not just do reference comparisons.  
A Unity Object is spit into two parts: the managed C# and the native C++ representations. Users interact with the managed object, and the engine manages the native object.  
When an Object is compared with null and it isn't *really* null, a check against the native object occurs. This makes it possible to Destroy an Object somewhere, and have an entirely different reference evaluate to null.  

This allows the Unity Editor to add extra context to null assignments occuring in the editor.  
If something is not assigned in the editor a custom Null Reference Exception, an [Unassigned Reference Exception](../Common%20Errors/Runtime%20Exceptions/Unassigned%20Reference%20Exception.md) is thrown.   
If there used to be an object, but it has been Destroyed or become invalid, you get a a  [Missing Reference Exception](../Common%20Errors/Runtime%20Exceptions/Missing%20Reference%20Exception.md).  
These exceptions also come bundled with some extra context, the location that is missing the object, none of which would be provided with a normal NRE.  
Due to this `GetComponent` will allocate memory in the Editor. Using `TryGetComponent` will not.  


#### Ramifications
The null conditional (`?.`) and the null coalescing (`??`, `??=`) operators will not function correctly with `UnityEngine.Object` types as these operators could not be overridden by Unity. When Unity was created these operators did not exist.  

If you want to reclaim memory on Destroyed objects you need to additionally ensure all references are set to null to allow the C# garbage collector to reclaim that memory exactly like other heap allocated types.  

---
This 2014 blog ["Custom == operator, should we keep it?"](https://blog.unity.com/technology/custom-operator-should-we-keep-it) goes into additional detail.  
The Resharper/Rider suggestion ["Possible unintended bypass of lifetime check of underlying Unity engine object"](https://github.com/JetBrains/resharper-unity/wiki/Possible-unintended-bypass-of-lifetime-check-of-underlying-Unity-engine-object) again details this further.  