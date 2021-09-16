### [Random.Range](https://docs.unity3d.com/ScriptReference/Random.Range.html)

#### Description
The most common issue with `Random.Range` is incorrect range parameters with the `int` version of the function.  
`Range` with `int` is `[minInclusive..maxExclusive)`. This means that the lower bound is included in the results, while the upper is not.

#### Resolution
Provide Range with an lower bound, and an upper bound `+1`.  
`array.Length` and `list.Count` are both `1` greater than the bounds of their collection, so they make fantastic limits.

##### Example
<<Code/Specific/Random/Range.rtf>>  