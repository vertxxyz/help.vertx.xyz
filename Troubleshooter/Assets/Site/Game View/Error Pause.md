# Error pause

The [Console window](https://docs.unity3d.com/Manual/Console.html) (**Window | General | Console**, <kbd>Ctrl+Shift+C</kbd>) has a setting called **error pause** that will pause the playing application when an error occurs.  

## Resolution
Disable **error pause** using the button at the top of the Console window.  
If error pause is already disabled, then search for [`Debug.Break`](https://docs.unity3d.com/ScriptReference/Debug.Break.html) in your code, and also make sure your IDE is not currently debugging.
