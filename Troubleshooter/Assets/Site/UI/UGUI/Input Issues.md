## Input issues: UGUI
Input in Unity UI (UGUI) requires multiple things to receive input events:
- There must be an **active [EventSystem](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/EventSystem.html)** in the Scene.  
    - You can create one via **GameObject | UI | Event System**  
- A **[Graphic Raycaster](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-GraphicRaycaster.html)** must be present on the Canvas and sub-Canvases above the element.  
- **Raycast target** must be enabled on the Graphic (eg. Image) attached to a the element that receives input (eg. Button).  
![Raycast Target](ui-raycast-target.png)
- Overlapping[^1] elements with **raycast target** enabled will block input.
- Overlapping[^1] elements that receive the same events will block input.  
   [Event Triggers](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-EventTrigger.html) will absorb all events, regardless of what is registered to them.
- **[Canvas Groups](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/class-CanvasGroup.html)** above the element must be marked Interactable.
- The EventSystem's [StandaloneInputModule](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-StandaloneInputModule.html) must have matching axes in the [Input Manager](https://docs.unity3d.com/Manual/class-InputManager.html) (**Edit | Project Settings | Input Manager**).  
    If this isn't properly set up, when you select the EventSystem its preview will say `no module`.

:::warning
Do not make assumptions. Double checking these steps are met is always recommended.
:::

To troubleshoot cases where other UI is blocking input, select the Event System and hover/click the UI that's failing. The preview pane at the bottom of the inspector should list the gameobject that received the event.

[^1]: Overlapping panels are below the element in the hierarchy.