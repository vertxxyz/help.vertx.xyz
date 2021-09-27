## Unity Event Method Requirements

The target method must:  
1. Be marked as `public`
2. Have none, or only **one** parameter  
   If the method has a parameter, it must be either a:
      - `float`
      - `int`
      - `bool`
      - `string`
      - `UnityEngine.Object` (or an inheriting type)
3. **Not** be marked as `static`
4. Have **no** return type. The method must have the return type `void`  

---  

[The method still doesn't appear in the functions list](Compile%20Errors.md)