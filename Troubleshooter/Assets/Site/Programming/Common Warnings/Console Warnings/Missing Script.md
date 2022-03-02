## Missing scripts
### Description
Missing Scripts can mean a number of things:  
- There was a script that was deleted.
- There was a script that was manually moved without its `.meta` file.
- The associated `.meta` file changed because of a merge or data corruption.
- A project was loaded without properly compiling, and Unity has not yet imported the script.
- The script or class it contains is improperly named.
- The script file no longer contains a MonoBehaviour or ScriptableObject.

### Resolution

If the script is no longer relevant, remove the placeholder component from the GameObject.  
You can find the GameObject by clicking the warning in the Console window.  
Otherwise follow [these](../../Scripts/Loading/Script%20Name.md) steps to set up the script correctly and ensure Unity is compiling the project.