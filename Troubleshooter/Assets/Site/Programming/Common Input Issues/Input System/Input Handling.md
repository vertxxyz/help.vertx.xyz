## Input System - Input Handling
### Description
When you have switched the Input System from the built-in input system to the new Input System there may be code that requires upgrading.  
### Resolution
See [migration from the old input system](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/index.html?subfolder=/manual/Migration.html) for information on how to upgrade your own code.  

If the code throwing this error is provided by Unity's UI you will need to replace the `StandaloneInputModule` with the new `InputSystemUIInputModule`. This can be found on the Event System GameObject found in the scene, and there may be an upgrade button on the component. See [UI Support](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/index.html?subfolder=/manual/UISupport.html) for more information.  
