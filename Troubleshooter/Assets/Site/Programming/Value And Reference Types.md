## Value and reference types

This is a short overview of value and reference behaviour. This description does not go over other topics like boxing, heap and stack memory, tuples, etc.

### Value types
`bool`, `char`, `float` and other [floating-point numeric types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types), `int` and other [integral numeric types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types), and `struct` types are value types.

Value types have value semantics, and each variable has its own copy of the data.  

```csharp
int a = 0;
int b = a;
a++;
// a != b
```  
When a value type is assigned to another, this copies by value.  
The value of `a`: `0`, was copied into `b`, and when `a` was incremented, `b` was unaffected.  

```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction: right

[a]->[1]
[b]->[0]
```

#### Methods
When a value type is returned via a method or property this is also a copy.  
Directly modifying the return value of these statements will not modify the original value. The compiler will throw a warning when you attempt this, because the logic performed is useless (see [CS1612](Common%20Errors/Compiler%20Errors/CS1612.md)).  
This behaviour can be modified in certain contexts with the [`ref`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref) or [`in`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier) keywords.

#### Comparisons
When two value types are compared, they are done so by value:  

```csharp
int a = 500;
int b = 500;
// a == b
```  

```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction: right
#.comp: fill=#8f8 stroke=#000
[a]->[<comp>500 ᵃ]
[b]->[<comp>500 ᵇ]
```

### Reference types
`class`, `interface`, `delegate`, and `record` types are reference types.  

```csharp
int[] a = {0, 1, 2};
int[] b = a;
a[0]++;
// a == b
```  

When a reference type is assigned to another, this copies by value, **but** importantly the value copied is a reference.  
The value of `a`: a location in memory (eg. `0x7fca1dbff861`), is assigned to `b`, `a` and `b` now point to the same value.  
Incrementing a value in `a` will also affect `b`.

```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction: right

[<reference>0x7fca1dbff861]
[a]->[0x7fca1dbff861]
[b]->[0x7fca1dbff861]
[0x7fca1dbff861]->[{1, 1, 2}]
```

#### Methods
When a reference type is returned or passed to a method or property this is also a copy of the reference.
This can trip up programmers who expect them to be different when their values were never copied.

#### Comparisons
Most[^1] reference types have complete reference semantics, and different variables can reference the same data.  
When two reference types are compared, the references are compared, not the values:  
```csharp
int[] a = {0, 1, 2};
int[] b = {0, 1, 2};
// a != b
```  
Although `a` and `b` may have the same value, they are not at the same place in memory, and are not equal.

```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction: right
#.comp: fill=#f88 stroke=#000 dashed

[<comp>0x7fca1dbff861]
[<comp>0xf058bcf5d100]
[a]->[0x7fca1dbff861]
[b]->[0xf058bcf5d100]
[0x7fca1dbff861]->[{0, 1, 2} ᵃ]
[0xf058bcf5d100]->[{0, 1, 2} ᵇ]
```

Not all reference types are compared this way, strings are compared by value.  
Often this should feel intuitive, when comparing complex structures you expect each instance to be different even when identical, when comparing simple types you expect them to be the same.  

## Null
Null is a reference that indicates a lack of an associated value. It has no location or members.
### Reference types
Reference types can be assigned to null. This is the default state of a reference type. Trying to access a value that is null will cause that code to fail at runtime, throwing a [Null Reference Exception](Common%20Errors/Runtime%20Exceptions/Null%20Reference%20Exception.md).  

```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction: right
#.x: visual=none stroke=#f55 body=bold

[<reference>null]
[<x>?]
[a]->[null]
[null]-->[?]
```

### Value types
By default, value types cannot be null, because they are always pointing directly to a value.  
Value types can be made to be nullable by appending `?` to the type. See [nullable value types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types).

```csharp
int? a = null; // System.Nullable<int>
if (a != null)
{
    Debug.Log($"{nameof(a)} is {a.Value}");
}
else
{
    Debug.Log($"{nameof(a)} does not have a value");
}
// a does not have a value
```

The implementation of this is a wrapper struct that contains the value: `value`, with a `bool`: `hasValue`, indicating whether the value is assigned or not. The compiler abstracts away null, and implements it through `hasValue` without hassle.
This setup means nullable types will have the same allocation behaviour as their original values.  

```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction: right

[<label>hasValue]
[<label>value]
[a]-[hasValue]
[a]-[value]
[hasValue]->[false]
[value]->[0]
```

[^1]: `string` and `record` are **not** value types, but have some value semantics like value equality. Certain custom types may also override comparisons to gain value equality.