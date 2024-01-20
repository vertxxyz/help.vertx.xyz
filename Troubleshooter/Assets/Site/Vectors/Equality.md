## Vector equality

Despite the Vector [`==` operator](https://docs.unity3d.com/ScriptReference/Vector3-operator_eq.html) using an approximate equality, you can encounter cases where two vectors appear equal due to the lack of precision in the [`ToString`](https://docs.unity3d.com/ScriptReference/Vector3.ToString.html) implementation, which uses two decimal places.

### Resolution
When logging vectors, provide enough precision with a format string[^1].
```diff
-Debug.Log(vector);
+Debug.Log(vector.ToString("F7"));
```
You can also provide a format in interpolated strings:
```diff
-Debug.Log($"the position is: {position}");
+Debug.Log($"the position is: {position:F7}");
```

### Alternatives
If you need even less precision when comparing vectors, use a distance comparison:
^^^
```csharp
if ((compareTo - position).sqrMagnitude < distance * distance)
{
    // "compareTo" and "position" are within "distance" of each other.
}
```
^^^ [`sqrMagnitude`](https://docs.unity3d.com/ScriptReference/Vector3-sqrMagnitude.html) is preferred when calculating the actual distance is unnecessary.

[^1]: See [standard numeric format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#FFormatString) for more information.
