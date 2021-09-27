## Managed Allocations in Jobs
### Description
Burst is a compiler that translates compiled IL bytecode to native code. It's designed to specifically operate on a subset of C# that Unity dubs "High Performance C#", or HPC#.  
HPC# is made up of [blittable types](https://docs.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types).
Burst's [language restrictions](https://docs.unity3d.com/Packages/com.unity.burst@latest/index.html?subfolder=/manual/docs/CSharpLanguageSupport_Types.html) are limited further,
but certain Unity-provided structures are provided to work within these restrictions.


### Resolution
Allocating managed memory in jobs is incredibly slow, and the job is not able to make use of the Unity Burst compiler to improve performance. So in almost all cases removing this allocation is the way forward.  

When using arrays or lists in Jobs you should be using NativeArray or NativeList, there are many different alternative collection types that can be found in the [Unity Collections Package](https://docs.unity3d.com/Packages/com.unity.collections@latest).  
These collections provide safety mechanisms for multithreading, and allocations are Burst-compiler ready.  

When performing many maths functions that may have managed allocations in certain APIs, there are often Burst-ready alternatives in the [Unity.Mathematics Package](https://docs.unity3d.com/Packages/com.unity.mathematics@latest).  

As a last resort, not Burst compiling a job will allow for allocation and use of managed types.  

---  
You can read more about this decision in the blog ["On DOTS: C++ & C#"](https://blog.unity.com/technology/on-dots-c-c)