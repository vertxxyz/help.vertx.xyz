## UnityEvent method requirements

The target method must:  
- Be marked as `public`
- Have none, or only **one** parameter  
  If the method has a parameter, it must be either a:
  - `float`
  - `int`
  - `bool`
  - `string`
  - `UnityEngine.Object` (or an inheriting type)
- **Not** return a value. The method must have the return type `void`  

---  

[The method still does not appear in the functions list.](Compiler%20Errors.md)