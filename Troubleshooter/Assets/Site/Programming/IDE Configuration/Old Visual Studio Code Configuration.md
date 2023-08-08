<<Abbreviations/UPM.md>>
## ![Visual Studio Code](/Images/vscode.svg) Old Visual Studio Code configuration

:::error  
As of August 2023 Microsoft has released a new version of the Unity extension for VS Code.  
See [the new configuration steps](Visual%20Studio%20Code%20Configuration.md) if you aren't stuck on an old version of Unity.  
:::

### Steps
:::note  
#### Windows
1. Install the [.NET SDK](https://dotnet.microsoft.com/download).
    1. Logout or restart Windows.
1. Install the [.NET Framework 4.7.1 **Developer Pack**](https://dotnet.microsoft.com/download/dotnet-framework/net471).
1. Install [MSBuild Tools](https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022).
1. Install the [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) from the VS Code Marketplace.
1. In the VS Code Settings editor (</kbd>Ctrl+,</kbd>), uncheck the C# extension's **Omnisharp: Use Modern Net** setting (`"omnisharp.useModernNet": false`).
1. Set the **External Script Editor** dropdown in Unity's External Tools preferences (**Edit | Preferences | External Tools**) to VS Code.  

:::  
:::note  
#### MacOS/Linux
1. Install the [.NET SDK](https://dotnet.microsoft.com/download).
1. Install the [.NET Framework 4.7.1 **Developer Pack**](https://dotnet.microsoft.com/download/dotnet-framework/net471).
1. Install [Mono with MSBuild](https://www.mono-project.com/download/preview/).
1. Install the [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) from the VS Code Marketplace.
1. In the VS Code Settings editor (</kbd>Ctrl+,</kbd>), uncheck the C# extension's **Omnisharp: Use Modern Net** setting (`"omnisharp.useModernNet": false`).
1. Set the **External Script Editor** dropdown in Unity's External Tools preferences (**Edit | Preferences | External Tools**) to VS Code.

:::
2. Install and update the [Visual Studio Code Editor](https://docs.unity3d.com/Manual/com.unity.ide.vscode.html) package in UPM (`com.unity.ide.vscode`).  
   As a last resort to troubleshooting setup try rolling back the package as some versions have introduced issues. You can always update it again if that fails.

### ⚠️ Warning

VS Code support is [limited](https://forum.unity.com/threads/update-on-the-visual-studio-code-package.1302621/).
- It's [debugger extension](https://marketplace.visualstudio.com/items?itemName=Unity.unity-debug) has been marked as deprecated.
- New versions of Unity will not have the Visual Studio Code Editor (`com.unity.ide.vscode`) package installed by default.

As VS Code is complex to configure, and is currently not receiving support for use with Unity, [Visual Studio](Visual%20Studio%20Code%20Configuration.md) or [JetBrains Rider](JetBrains%20Rider%20Configuration.md) are recommended instead.