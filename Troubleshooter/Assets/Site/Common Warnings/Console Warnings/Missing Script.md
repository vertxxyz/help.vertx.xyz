# Missing scripts

Missing Scripts can mean a number of things:  
- The script was deleted.
- The script was manually moved without moving its `.meta` file.
- The associated `.meta` file changed because of a merge or data corruption.
- A project was loaded without properly compiling, and Unity has not yet imported the script.
- The script or class it contains is improperly named.
- The script file no longer contains a MonoBehaviour or ScriptableObject.

## Resolution

You can find the GameObject by clicking the warning in the Console window. Note that it may be an asset or prefab; move the Console window so the Project window is also visible, and try again.  
I also have provided a script to search complete projects for missing scripts in prefabs and scenes[^1].


### The script has been deleted on purpose
If the script is no longer relevant, remove the placeholder component from the GameObject.  

If you're unsure what script it was, you may have luck looking at the [YAML](https://docs.unity3d.com/Manual/FormatDescription.html) of the scene or asset. Attempt to identify recognisable features like variables names, then use them to search your project for the missing script.

If you can't actually remove the component from the object, you can run [`GameObjectUtility.RemoveMonoBehavioursWithMissingScript`](https://docs.unity3d.com/ScriptReference/GameObjectUtility.RemoveMonoBehavioursWithMissingScript.html) to get rid of them[^1].

### Fixing issues with an existing script
If the script is in the project but can't be added to an object, follow [these steps](../../Script%20Loading%20Issues.md) to correct those issues.  

### Reassigning a script
You can re-assign a missing script by dragging a valid MonoBehaviour or ScriptableObject script file into the missing Script field; variables from the previously assigned script may be recovered if done correctly.

[^1]: [`FindMissingObjects` editor script](https://gist.github.com/vertxxyz/0c3843b279ac821fe9f5b9b30c4a292c#file-findmissingobjects-cs) that adds 3 menu items under **Tools | Missing Objects**.
