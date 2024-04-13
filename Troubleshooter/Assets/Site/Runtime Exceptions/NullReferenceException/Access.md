# NullReferenceException: Access
Now we understand [what line is throwing the exception](Stack%20Trace.md), and [what variables can be null](Reference%20Types.md), we must understand access.  
Access is denoted by:
- `variable.`  - member access.
- `variable[]` - array element or indexer access.
- `variable()` - invocation.

Only **accessing** a reference type can throw a `NullReferenceException`. `null` is a valid state, but accessing `null` is not.

---  

[I understand what reference types are being accessed.](Debugging.md)
