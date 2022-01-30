## NullReferenceException: Reference types
Now we understand [stack traces](Stack%20Trace.md), we know what line is throwing the `NullReferenceException`.  

On this line, only **reference types** can be `null`:  
:::note  
`class`, `interface`, and `delegate` types are **reference types**.  
`struct` is **not**.  
:::  

Usually, this can be reduced to *complex*, and *simple* types:  
*Complex* types tend to be reference types, `Component`, `GameObject`, `List`, etc.  
*Simple* structures tend to be structs, `int`, `float`, `Vector3`, `Quaternion`, etc.  

You can double check types by using the [documentation](https://docs.unity3d.com/ScriptReference/) or IDE inspection.  
^^^
![Struct in the ScriptReference](struct-documentation.png)  
^^^ Documentation indicating `struct` inheritance.

You can find a more detailed breakdown of reference types [here](../../../Value%20And%20Reference%20Types.md).

---  

[I understand what reference types are.](Access.md)