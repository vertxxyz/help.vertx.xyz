<<Abbreviations/Asmdef.md>>
# Assembly Definitions
Editor code isn't accessible in a build. Unity has multiple methods to separate and not include this code.

## Resolution
## Editor assemblies
:::warning
Editor folders don't function under Assembly Definitions
:::

It is recommended to have a Runtime and Editor asmdefs separated into different folders at a root level, unlike the usual paradigm of having editor folders scattered throughout your project.  

An editor asmdef should only include **Editor** under the Platforms selection.  
Check that **Any Platform** is switched **off**, *Deselect All*, and then tick **Editor** only.

![Editor Assembly Definition](editor-asmdef.png)

### Preprocessor directives
Editor code can be wrapped in a `UNITY_EDITOR` preprocessor `#if` directive.  
This code will only compile when the preprocessor argument is present, which Unity defines when running code from the editor.  
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

### Replacing UnityEditor code
If you are finding a need to include editor code in a build then you are abusing the Editor APIs. There is likely a runtime API that will achieve the same outcome, though often requiring completely different methods. Research what you're trying to achieve and implement a solution that does not involve Editor APIs.

For example, [SceneAsset](https://docs.unity3d.com/ScriptReference/SceneAsset.html) is in UnityEditor. Referring to Scenes directly via their asset in runtime code is incorrect. The correct ways to refer to scenes are via their names (a `string`), their build index (an `int`), or by loading them with a wrapper like [Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@latest/index.html?subfolder=/manual/LoadSceneAsync.html).  
There are also third-party solutions like [this one](https://github.com/starikcetin/Eflatun.SceneReference) that allow indirect references to scenes via their assets using the inspector.
