### Property Decorators on Properties

#### Description
A decorator like `[Header]` must be decorating a field that Unity already serializes and is displaying in the inspector.

#### Resolution
Instead of decorating a field that does not serialize (like a property), decorate a serializable field.  
That is a field that is marked with `[SerializeField]`, or is `public`.  
If your field is not visible, follow the debugging steps [here](../../../Programming/Variables/Serialization%20First.md).