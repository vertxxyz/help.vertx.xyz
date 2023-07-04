## Serialized references
### How-to
- [Referring to objects **from the scene**.](Serializing%20Component%20References.md)
- [Referring to objects **between scenes**.](Cross-Scene%20References.md)
- [Referring to prefab assets, and instancing them.](References%20To%20Prefabs.md)
- [Referring to objects in the scene **from a prefab**.](Prefabs%20Referencing%20Components.md)
- [Referring to children of prefabs.](References%20To%20Prefab%20Children.md)

For a visual representation of valid serialized references see [this diagram](Valid%20References.md).
### Issues
First use the how-to guide to understand whether your reference is valid.
- [I cannot drag a reference into an object field.](Assignment%20Issues.md)

### What is serialization?
> #### [Serialization, from the Unity manual](https://docs.unity3d.com/Manual/script-Serialization.html)
> Serialization is the automatic process of transforming data structures or GameObject states into a format that Unity can store and reconstruct later.

Serialization is saving data. In the context of Unity's serialization, this generally means it's editable via the inspector. You can open `.asset` or `.scene` files as text and see what's being serialized long-term; the serialization format used by Unity is called [YAML](https://docs.unity3d.com/Manual/UnityYAML.html).