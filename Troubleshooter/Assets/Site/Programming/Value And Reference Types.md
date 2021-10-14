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
When a value type is assigned to another, this assignment will copy by value. This means that the value of `a`, `0`, was copied into `b`, and when `a` was incremented, `b` was unaffected.  
`a` is now `1`, but `b` has remained `0`.  

When a value type is returned via a method or property this is also a copy. This means that directly modifying the return value of these statements will not modify the original value. The compiler will throw a warning when you attempt this, because the logic performed is useless (see [CS1612](Common%20Errors/Compiler%20Errors/CS1612.md).)  
These values are copied when passed to a method or a property. This behaviour can be modified with methods with the `ref` or `in` keywords.

When two value types are compared, they are done so by value:  

```csharp
int a = 500;
int b = 500;
// a == b
```  

### Reference types
`class`, `interface`, `delegate`, and `record` types are reference types.  

```csharp
int[] a = {0, 1, 2};
int[] b = a;
a[0]++;
// a == b
```  

When a reference type is assigned to another, this assignment will also copy by value, **but** importantly the value copied is a reference. A reference is a *pointer* to a location in memory.
This means that the location of `a`, a location in memory (something like `0x7fca1dbff861`) is assigned to `b`, `a` and `b` now point to the same value.  
`a` and `b` are both `1, 1, 2`.

When a reference type is returned or passed to a method or property this is also a copy of the reference. This can trip up programmers who assign or call methods with classes or arrays, expecting them to be different when they were never copied.  

Most[^1] reference types have complete reference semantics, and different variables can reference the same data.  

When two reference types are compared, the references are compared, not the values:  
```csharp
int[] a = {0, 1, 2};
int[] b = {0, 1, 2};
// a != b
```  
Although `a` and `b` may have the same value, they are not at the same place in memory, and are not equal.
Not all reference types are compared this way, strings are compared by value.  
Often this should feel intuitive, when comparing complex structures you expect each instance to be different even when identical, when comparing simple types you expect them to be the same.  

[^1]: `string` and `record` are **not** value types, but have some value semantics like value equality. Certain custom types may also override comparisons to gain value equality.