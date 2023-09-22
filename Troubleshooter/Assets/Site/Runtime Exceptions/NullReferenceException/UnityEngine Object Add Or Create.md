## NullReferenceException: UnityEngine.Object — Add or Create

:::note
#### 1. Do not override the reference
- Remove any assignments that could be overriding your reference.
- Ensure nothing is destroying the Object, or setting it to `null` before you attempt to use it.

:::

:::note
#### 2. Do not use a modern null-checking operator
Check that you are not using modern null-checking operator (`?.`, `??`, `??=`).
See [Unity null](../../Unity%20Null.md) to learn about the specifics surrounding null and UnityEngine.Object types.
:::
