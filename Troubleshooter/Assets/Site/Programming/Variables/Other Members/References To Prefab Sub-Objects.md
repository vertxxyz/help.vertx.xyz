### References to Prefab sub-objects
You cannot directly reference sub-objects of a prefab externally.  
Instead, go through a script on the root of the prefab that contains the references.

Follow these steps to reference a sub-object on a prefab instance:  
1. Create a script to reference the object you care about, and add it to the root of the prefab.
2. [Configure that script with a reference to the object](Serializing%20Component%20References.md).
3. See [referencing Prefabs from Scenes](References%20To%20Prefabs.md) to learn how to reference scripts on the root of a prefab. 
4. Instance the prefab, and cache the instance returned by [`Instantiate`](https://docs.unity3d.com/ScriptReference/Object.Instantiate.html).
5. Use the instance (now the script on the root of the prefab instance) to access the instanced sub-object.