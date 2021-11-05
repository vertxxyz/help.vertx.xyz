## UnityEvent method requirements

The target method must:  
1. Be marked as `public`
2. Have none, or only **one** parameter  
   If the method has a parameter, it must be either a:
      - `float`
      - `int`
      - `bool`
      - `string`
      - `UnityEngine.Object` (or an inheriting type)
3. **Not** return a value. The method must have the return type `void`  

---  

[The method still does not appear in the functions list](Compile%20Errors.md)