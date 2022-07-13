<<Abbreviations/UPM.md>>
## Visual Studio Code configuration
### Steps
1. Follow **all** of the [configuration steps](https://code.visualstudio.com/docs/other/unity).  
Where it mentions the External Tools preferences, these are **in Unity**.  
1. Install the [.NET Framework 4.7.1 **Developer Pack**](https://dotnet.microsoft.com/download/dotnet-framework/net471).
1. Set `omnisharp.useModernNet` to `false` in your VS Code settings and restart OmniSharp.
1. Ensure a full Framework runtime and MSBuild tooling is installed:
   1. **Windows:** Install .NET Framework along with [MSBuild Tools](https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022)
   1. **MacOS/Linux:** Install [Mono with MSBuild](https://www.mono-project.com/download/preview/)
1. Install and update the [Visual Studio Code Editor](https://docs.unity3d.com/Manual/com.unity.ide.vscode.html) package in UPM (`com.unity.ide.vscode`).  
As a last resort to troubleshooting setup try rolling back the package as some versions have introduced issues. You can always update it again if that fails.