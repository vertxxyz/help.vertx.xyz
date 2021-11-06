## Missing scripts
### Description
Missing Scripts can mean a number of things:  
- There was a script that was deleted
- There was a script that was manually moved without its `.meta` file
- A project was loaded without properly compiling, and Unity has not yet imported the script
- The script or class it contains is improperly named
- The script file no longer contains a MonoBehaviour or ScriptableObject

### Resolution

If the script is no longer relevant, remove the placeholder component from the GameObject.  
You can find the GameObject by clicking the warning in the Console window.  
Otherwise follow [these](../Scripts/1%20Script%20Loading.md) steps to sure the script you expect to be functioning is set up correctly and Unity is compiling the project.