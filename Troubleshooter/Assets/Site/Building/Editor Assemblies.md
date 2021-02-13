### Editor Assemblies
Editor code is not accessible in a build. Unity has multiple methods to separate and not include this code.

#### Editor Folders
Code placed in a folder named **Editor** will be compiled into an editor assembly. These folders can be placed anywhere in your project.   
You can find more information regarding special folder names [here](https://docs.unity3d.com/Manual/SpecialFolders.html).

#### Preprocessor Directives
Editor code can be wrapped in a `UNITY_EDITOR` preprocessor `#if` directive.  
This code will only compile when the preprocessor argument is present, which Unity only defines when running code from the editor.

```csharp
#if UNITY_EDITOR
using UnityEditor;
#endif

// General Code

#if UNITY_EDITOR
// Other editor-related code
#endif
```