### Input Issues With UI
Input in UGUI (Unity UI that uses a Canvas) requires multiple things to receive input events.  

1. There must be an [EventSystem](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/EventSystem.html) in the scene.  
    - You can create one via **GameObject/UI/Event System**  
2. Canvases must have a [Graphic Raycaster](https://docs.unity3d.com/Packages/com.unity.ugui@latest/index.html?subfolder=/manual/script-GraphicRaycaster.html) component on them.  
3. The Graphic (eg. an Image) attached to a component that receives input (eg. a Button) must have **Raycast Target** enabled.  
![Raycast Target](http://help.vertx.xyz/Images/ui-raycast-target.png)
4. There mustn't be other UI receiving the same events below in the hierarchy. EventTriggers will absorb all events, regardless of what is registered to their events.