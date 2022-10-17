## NullReferenceException: UnityEngine.Object — Add or Create

1. Check that you are not using modern null checking operators[^1] (`?.`, `??`, `??=`).
1. Ensure that nothing is destroying the Object, or setting it to `null` before you attempt to use it.  


[^1]: See [Unity null](../../../Other/Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.  