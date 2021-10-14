## Input Issues - UGUI
Input in UGUI (Unity UI that uses a Canvas) requires multiple things to receive input events.  

1. There must be an **active [EventSystem](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/EventSystem.html)** in the Scene.  
    - You can create one via **GameObject | UI | Event System**  
2. A **[Graphic Raycaster](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-GraphicRaycaster.html)** must be present on the Canvas and sub-Canvases.  
3. **Raycast Target** must be enabled on the Graphic (eg. Image) attached to a component that receives input (eg. Button).  
![Raycast Target](ui-raycast-target.png)
4. There mustn't be other UI receiving the same events below in the hierarchy.  
   [Event Triggers](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-EventTrigger.html) will absorb all events, regardless of what is registered to them.

:::warning
Do not make assumptions. Double checking these steps are met is always recommended.
:::