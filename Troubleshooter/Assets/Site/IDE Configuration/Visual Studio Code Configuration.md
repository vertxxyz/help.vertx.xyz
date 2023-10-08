<<Abbreviations/UPM.md>>
## ![Visual Studio Code](/Images/vscode.svg) Visual Studio Code configuration

As of August 2023 Microsoft has released a new version of the Unity extension for VS Code.
The new extension relies on the [C# Dev Kit](https://learn.microsoft.com/en-us/visualstudio/subscriptions/vs-c-sharp-dev-kit). Do note that this is unlike the previous extensions, so now it falls under a [new license](https://marketplace.visualstudio.com/items/ms-dotnettools.csdevkit/license) similar to Visual Studio Community's.

### Steps

#### Follow **all** of the [configuration steps](https://code.visualstudio.com/docs/other/unity):
1. [Install a Unity version](../Unity%20Hub/Editor%20Installation.md) greater than or equal to 2021+.
1. Ensure the [Visual Studio Editor](https://docs.unity3d.com/Manual/com.unity.ide.visualstudio.html) package installed and updated to at least `2.0.20` in UPM (`com.unity.ide.visualstudio`).
   :::error{.small}
   This is **not** the Visual Studio Code Editor package, if it is installed you should remove it.
   :::
1. [Install](https://code.visualstudio.com/docs/editor/extension-marketplace) the [Unity for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=visualstudiotoolsforunity.vstuc) extension.
1. Set the **External Script Editor** dropdown in Unity's External Tools preferences (**Edit | Preferences | External Tools**) to Visual Studio Code.

### If you are experiencing issues:
- If you have compiler errors, if possible [comment out](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/comments) those files so Unity can compile code.
- Check your C# and C# Dev Kit extensions are upgraded to the latest versions.
- Install the [.NET SDK](https://dotnet.microsoft.com/download).
  - Logout or restart Windows.
- Restart VS Code.
- Open the project from Unity's **Assets | Open C# Project** menu.
- Restart your computer.

---

- See [old Visual Studio Code configuration](Old%20Visual%20Studio%20Code%20Configuration.md) for the steps for the old extension.
- [Return to general IDE configuration.](../IDE%20Configuration.md)
