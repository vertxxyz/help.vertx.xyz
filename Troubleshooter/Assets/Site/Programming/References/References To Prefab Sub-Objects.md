## References to Prefab sub-objects
You cannot directly reference sub-objects of a prefab externally.  
Instead, create a script on the root of the prefab that contains the references.

#### Follow these steps to reference a sub-object on a prefab instance:  
1. Create a script to reference the object you care about, and add it to the root of the prefab.
2. [Configure that script with a reference to the sub-object](Serializing%20Component%20References.md).
3. See [referencing Prefabs from Scenes](References%20To%20Prefabs.md) to learn how to reference scripts on the root of a prefab. 
4. Instance the prefab, and cache the instance returned by [`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html).
5. Use the instance (now the script on the root of the prefab instance) to access the instanced sub-object.

You can view the component on the root of the prefab as a manager, handling any interactions with its sub-objects. Create methods on this component to manage those objects, compartmentalising interactions instead of modifying things directly.

```nomnoml
<<Nomnoml/shared.nomnoml>>

[<label>prefab]
[<label>subObject]

[In-Scene Component]-[prefab]
[prefab]->[Prefab Root Component]
[Prefab Root Component]-[subObject]
[subObject]->[Prefab Sub-object]

```