# ArgumentException: Unknown Type

1. If you are trying to use aspects in a `WithAll` you should use `WithAspect` instead.
1. Aspects may not be compatible with `WithChangeFilter`, target the individual components instead.
1. If you are using generic components, you need to register each concrete type you use with [`[RegisterGenericComponentType]`](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.RegisterGenericComponentTypeAttribute.html).  
   ```csharp 
   [assembly: RegisterGenericComponentType(typeof(MyGenericComponent<int>))]
   [assembly: RegisterGenericComponentType(typeof(MyGenericComponent<float>))]
   ```
