# Property decorators on properties
A decorator like `[Header]` must be applied to fields that Unity serializes and displays in the inspector.

## Resolution
Don't decorate variables that aren't serialized, a property for example; instead decorate a serializable field.  
That is a field that is marked with `[SerializeField]`, or is `public`.  

```diff
-[Header("Example")]
-public Example Value { get; private set; }

+[Header("Example")]
+[SerializeField] private Example _value;
```

---

If your field isn't visible, follow the debugging steps [here](../../Serialization.md).
