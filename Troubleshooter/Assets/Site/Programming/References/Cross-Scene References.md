## Cross-scene references

:::warning
Serialized references between objects cannot occur across scenes.
:::

Cross-scene references are disallowed as they cannot be saved in Scene files and will also cause [NullReferenceExceptions](../../NullReferenceException.md) when both scenes are not loaded in the editor.

### Resolution
If you cannot change your architecture to avoid these references, consider constructing them in code, not serializing them directly.  

::::note  
#### Some options include:
- Finding the target via a scene query ([`Physics`](https://docs.unity3d.com/ScriptReference/Physics.html) queries for example) and using [GetComponent methods](GetComponent%20Methods.md).
- Serialize a reference to the target to a [singleton](Singletons.md) in the target scene, then use the singleton to get the target.
- Have the target register itself to a [singleton](Singletons.md) on startup, then use the singleton to get the target.
- Serialize GUIDs which are later resolved in code via a [registry you create yourself](https://blog.unity.com/engine-platform/spotlight-team-best-practices-guid-based-references).
- Using a [Scriptable Object architecture](https://www.youtube.com/watch?v=raQ3iHhE_Kk).
- Using a dependency injection framework.

Each of these approaches have benefits and drawbacks, including project and code maintenance overhead. Some have pre-built approaches that you might find on a repository host or [OpenUPM](https://openupm.com).  
::::  

### Notes
If you are avoiding having too many things in a single scene to prevent merge conflicts, consider breaking areas down into prefabs, and make sure you have [smart merge](https://docs.unity3d.com/Manual/SmartMerge.html) configured.

It's generally undesirable to bypass this restriction with [`EditorSceneManager.preventCrossSceneReferences`](https://docs.unity3d.com/ScriptReference/SceneManagement.EditorSceneManager-preventCrossSceneReferences.html) unless you are making editor-only tooling that requires it.