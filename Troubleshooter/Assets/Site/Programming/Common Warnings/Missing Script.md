### Missing Scripts
#### Description
Missing Scripts can mean a number of things:  
1. There was a script that was deleted
2. There was a script that was manually moved without its `.meta` file
3. A project was loaded without properly compiling, and Unity has not yet imported the script
4. The script or class it contains is improperly named
5. The script file no longer contains a MonoBehaviour or ScriptableObject

#### Resolution

If the script is no longer relevant, remove the placeholder component from the GameObject.  
Otherwise follow [these](../Scripts/1%20Script%20Loading.md) steps to sure the script is set up correctly and Unity is compiling the project.