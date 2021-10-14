## UnityEvent Property Requirements

The target property must:
1. Be marked as `public`
2. It must be either a:
    - `float`
    - `int`
    - `bool`
    - `string`
    - `UnityEngine.Object` (or an inheriting type)
3. **Not** be marked as `static`

---  

[The property still does not appear in the functions list](Compile%20Errors.md)