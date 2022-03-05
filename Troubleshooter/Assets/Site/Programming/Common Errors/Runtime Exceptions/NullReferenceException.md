## NullReferenceException
### Description
A `NullReferenceException` (NRE) occurs when code tries to access members of a variable which has no value assigned.  

```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction: right
#.x: visual=none stroke=#f55 body=bold

[<reference>null]
[<x>?]
[variable]->[null]
[null]-->[?]
```

:::warning
Declaring a reference variable does not automatically assign it a value.
:::

### Troubleshooting

- [I don't understand stack traces.](NullReferenceException/Stack%20Trace.md)
- [I don't understand reference types.](NullReferenceException/Reference%20Types.md)
- [I don't understand access.](NullReferenceException/Access.md)
- [I don't understand debugging.](NullReferenceException/Debugging.md)
- [I understand all of the above.](NullReferenceException/Options.md)