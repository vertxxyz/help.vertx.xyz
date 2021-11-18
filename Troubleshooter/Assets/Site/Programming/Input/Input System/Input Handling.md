## Input System: Input handling
### Description
When you have switched the input handling from the built-in Input Manager to the new Input System there may be code that requires upgrading.  
### Resolution
#### Migration
See [migration from the old input system](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/index.html?subfolder=/manual/Migration.html) for information on how to upgrade your own code, or follow a tutorial [like this one](https://learn.unity.com/project/using-the-input-system-in-unity) to learn how to use the Input System generally.  

#### UI Issues
If the code throwing this error is provided by Unity's UI you will need to replace the `StandaloneInputModule` with the new `InputSystemUIInputModule`. This can be found on the Event System GameObject found in the scene, and there may be an upgrade button on the component. See [UI Support](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/index.html?subfolder=/manual/UISupport.html) for more information.  

#### Revert to the old Input Manager
If you switched to the new Input System and want to switch back to the old one:
1. Switch the active input handler.
   1. Go to **Edit | Project Settings | Player | Other Settings | Configuration**.
   2. From **Active Input Handling** select **Input Manager (Old)**.
2. Remove the Input System package.
   1. Go to **Window | Package Manager**.
   2. Go to the **Packages: In Project** view (the second toolbar menu).
   3. Select the **Input System** package.
   4. Remove the package via the button in the bottom right.