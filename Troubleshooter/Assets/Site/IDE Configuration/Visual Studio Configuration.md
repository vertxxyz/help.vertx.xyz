<<Abbreviations/UPM.md>>
## ![Visual Studio](/Images/visualstudio.svg) Visual Studio configuration
### Installation and configuration
#### Visual Studio was installed with the Unity Hub
Visual Studio will have the Unity workload installed.
Follow the [configuration instructions](https://docs.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity#configure-unity-to-use-visual-studio) to configure the remaining setup.

#### Visual Studio was installed manually
Follow the [configuration instructions](https://docs.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity#install-unity-support-for-visual-studio), including installation of the Unity workload.

#### Installing Visual Studio for the first time
::::note
#### Install Visual Studio via the Unity Hub
Installing Visual Studio via the Unity Hub means it's mostly ready for use with Unity.

:::note
#### No Unity versions are installed
Tick **Microsoft Visual Studio Community** during the installation process in the Unity Hub. (**Installs | Install Editor**)
:::
:::note
#### A Unity version is installed
See [Unity Hub: Module installation](../Unity%20Hub/Module%20Installation.md) and install **Microsoft Visual Studio Community**.
:::
Follow the [configuration instructions](https://docs.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity#configure-unity-to-use-visual-studio) to check the remaining setup.
::::
::::note
#### I want to install Visual Studio manually
Follow all of the instructions listed on [this page](https://docs.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity).
::::

### If you are experiencing issues:

- If you have compiler errors, if possible [comment out](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/comments) those files so Unity can compile code.
- Ensure the [Visual Studio Editor](https://docs.unity3d.com/Manual/com.unity.ide.visualstudio.html) package is installed and updated in UPM (`com.unity.ide.visualstudio`).
- Regenerate project files via Unity.
  1. Close VS.
  1. Select **regenerate project files** in **Edit | Preferences | External Tools**.
  1. Reopen VS via **Assets | Open C# Project**.
- Regenerate project files via VS.
  - If an assembly in the Solution Explorer is marked as **(incompatible)**, right-click it and select **reload with dependencies**.
- Restart your computer.

---

[Return to general IDE configuration.](../IDE%20Configuration.md)
