## Input issues: UGUI
Input in Unity UI (UGUI) requires multiple things to receive input events:
- There must be an **active [EventSystem](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/EventSystem.html)** in the Scene.  
    - You can create one via **GameObject | UI | Event System**  
- A **[Graphic Raycaster](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-GraphicRaycaster.html)** must be present on the Canvas and sub-Canvases.  
- **Raycast Target** must be enabled on the Graphic (eg. Image) attached to a component that receives input (eg. Button).  
![Raycast Target](ui-raycast-target.png)
- There mustn't be other UI receiving the same events below in the hierarchy.  
   [Event Triggers](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-EventTrigger.html) will absorb all events, regardless of what is registered to them.
- The EventSystem's [StandaloneInputModule](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-StandaloneInputModule.html) must have matching axes in the [Input Manager](https://docs.unity3d.com/Manual/class-InputManager.html) (**Edit | Project Settings | Input Manager**).  
    If this is not properly set up, when you select the EventSystem its preview will say `no module`.

:::warning
Do not make assumptions. Double checking these steps are met is always recommended.
:::