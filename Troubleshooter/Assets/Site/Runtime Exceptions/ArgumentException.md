## ArgumentException

Argument exceptions are thrown when what is passed to a method is invalid.  
The error should describe what went wrong, and often how to fix it.  

Be aware that [extension methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) can throw this exception when the object you invoke the method on is invalid.

### Examples
#### Input System
- [ArgumentException: Input Button X is not setup.](../Input/Built-In%20Input/Input%20Manager.md)
- [ArgumentException: Input Axis X is not setup.](../Input/Built-In%20Input/Input%20Manager.md)
- [ArgumentException: Input Key named: X is unknown.](../Input/Built-In%20Input/Key%20Conventions.md)

---

#### ArgumentNullException
See [ArgumentNullException](ArgumentNullException.md).
