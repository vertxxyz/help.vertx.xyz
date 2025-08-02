---
title: "Input handling"
description: "Input code differs between the built-in Input Manager and the Input System, and changes are required."
---
# Input System: Input handling

```
InvalidOperationException: You are trying to read Input using the UnityEngine.Input class, but you have switched active Input handling to Input System package in Player Settings.
```

Input code differs between the built-in [Input Manager](https://docs.unity3d.com/Manual/class-InputManager.html) and the [Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/), and changes are required.  

:::warning
As of Unity 6.1 the Input System is the default method of input handling.
:::

## Resolution
Check the [stack trace](../../Stack%20Traces.md) to determine the origin of the error. Then choose one:

::::note
### The error was thrown from Unity UI
Replace the **StandaloneInputModule** with an [`InputSystemUIInputModule`](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/index.html?subfolder=/manual/UISupport.html) . This can be found on the **Event System** GameObject found in the scene, and there may be an upgrade button on the component.

If you do not have an Event System, create one via **GameObject | UI | Event System**, then upgrade it.

::::
::::note
### The error was thrown from user code
Choose one:    
:::note  
### Migrate your code to the Input System
See [migration from the old input system](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/index.html?subfolder=/manual/Migration.html) for information on how to upgrade your own code, or follow a tutorial [like this one](https://learn.unity.com/project/using-the-input-system-in-unity) to learn how to use the Input System generally.  
::: 
:::note  
### Revert to the old Input Manager
If you switched to the Input System and want to switch back to the old Input Manager.  
1. Go to **Edit | Project Settings | Player | Other Settings | Configuration**.
2. From **Active Input Handling** select **Input Manager (Old)**.

^^^{.foldout}
1. Go to **Window | Package Manager**.
2. Go to the **Packages: In Project** view.
3. Select the **Input System** package.
4. Remove the package via the button in the bottom right.
^^^Remove the Input System package (optional).

:::
:::note
### Enable both methods of input handling
If you're following a tutorial and don't mind using either system, your project can support both.  
1. Go to **Edit | Project Settings | Player | Other Settings | Configuration**.
2. From **Active Input Handling** select **Both**.

This method isn't recommended for long term projects.

:::
::::
