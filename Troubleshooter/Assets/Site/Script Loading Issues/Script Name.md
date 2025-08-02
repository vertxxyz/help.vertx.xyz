# Class and file names

:::info  
In Unity 6 and above this restriction has been lifted.  
It remains good practice and convention to file and script names.  
See [script file names](https://docs.unity3d.com/Manual/naming-scripts.html) for more information.  
:::

The name of your class must be **identical** to its file name.  

You can find the name of your class where your code looks like:  
<<Code/Scripts/Script Loading 1.html>>

In this example if your script isn't named ::![Script Icon](script-icon.svg){.inline} `ClassName`::{.note}, you must rename it to be identical.  

:::warning{.small}
This includes **capitalisation** and **spaces**.
:::  

## Notes
- There should only be one `MonoBehaviour` or `ScriptableObject` in a file so it can be correctly associated with the file.  
- C# uses pascal case (<kbd>PascalCase</kbd>) as its [naming convention](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names#naming-conventions) for types. If your class is lowercase, it's not following this convention.
- Unity will hide the file extension, but it should be `.cs`.


---  
[My script still cannot be loaded.](Console%20Errors.md)
