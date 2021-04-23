### Serializing Generic Types
#### 2020.1.0a8+
The serializer can now serialize fields of generic types directly; it is no longer necessary to derive a concrete subclass from a generic type in order to serialize it.

#### Pre-2020.1
Generic types must be a derived concrete subclass.  
This means that if any type that contains a generic in its definition needs to be inherited from plainly to become serializable.

##### Example
<<Code/Serialization/Serializing Generics.rtf>>