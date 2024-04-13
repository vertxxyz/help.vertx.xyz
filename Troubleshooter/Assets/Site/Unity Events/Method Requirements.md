# UnityEvent method requirements

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
- [I want to learn to work around this restriction.](Method%20Requirements%20Workaround.md)
- [I have changed my method and it still doesn't appear in the functions list.](Compiler%20Errors.md)
