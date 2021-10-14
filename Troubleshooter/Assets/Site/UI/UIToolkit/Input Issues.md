## Input Issues - UIToolkit
Input in UIToolkit requires multiple things to receive input events.

1. There must be an **active [EventSystem](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/EventSystem.html)** in the Scene.
    - You can create one via **GameObject | UI | Event System**
2. **Picking Mode** must be set to Position on the Visual Element.  
3. There mustn't be other elements receiving the same events below in the hierarchy.  
   Visual Elements have Picking Mode enabled by default, this can block events.
4. If using the Input System package, it is required to be version `1.1.0-pre.5` or above.

:::warning
Do not make assumptions. Double checking these steps are met is always recommended.
:::