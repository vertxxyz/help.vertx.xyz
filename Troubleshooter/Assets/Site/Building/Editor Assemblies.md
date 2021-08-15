### Editor Assemblies
#### Description
Editor code is not accessible in a build. Unity has multiple methods to separate and not include this code.

#### Resolution
##### Editor Folders
Code placed in a folder named **Editor** will be compiled into an editor assembly. These folders can be placed anywhere in your project.  
The code in these assemblies will not be present in the build.  
You can find more information regarding special folder names [here](https://docs.unity3d.com/Manual/SpecialFolders.html).

##### Preprocessor Directives
Editor code can be wrapped in a `UNITY_EDITOR` preprocessor `#if` directive.  
This code will only compile when the preprocessor argument is present, which Unity only defines when running code from the editor.  
The surrounded code will be stripped when building the game.

```csharp
#if UNITY_EDITOR
using UnityEditor;
#endif

// General Code

#if UNITY_EDITOR
// Other editor-related code
#endif
```

The result when the preprocessor is stripped must also be valid code to compile.  

##### Replacing UnityEditor code
If you are finding a need to include editor code in your build then you are abusing the Editor APIs. There is likely a runtime API that will achieve the same outcome, albeit sometimes with completely different methods. Research what you're trying to achieve and implement a solution that does not involve Editor APIs.  
For example, [SceneAsset](https://docs.unity3d.com/ScriptReference/SceneAsset.html) is in UnityEditor. If you are referring to scenes directly via their asset in runtime code, this is incorrect. The correct ways of referring to scenes are via their names (a `string`), their build index (an `int`), or by loading them with a wrapper like [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@latest/index.html?subfolder=/manual/LoadSceneAsync.html).  
There are also third-party solutions like [this one](https://github.com/JohannesMP/unity-scene-reference) that allow you to indirectly reference scenes via their assets using the inspector.  