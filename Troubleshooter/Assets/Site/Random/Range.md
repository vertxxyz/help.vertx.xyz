## [Random.Range](https://docs.unity3d.com/ScriptReference/Random.Range.html)

`Range` with `int` is `[minInclusive..maxExclusive)`. The upper bound isn't included in the results.

```csharp
// Always true.
Random.Range(0, 1) == 0
```

### Resolution
:::note  
#### Collections
`array.Length` and `list.Count` are both `1` greater than the bounds of their collection, so they make fantastic limits.

#### Example
<<Code/Specific/Random/Range.rtf>>  

:::

:::note
#### Custom ranges
Provide Range with the lower bound, and an upper bound `+ 1`.

#### Example
```csharp
// Returns [0..max].
int randomValue = Random.Range(0, max + 1);
```

:::