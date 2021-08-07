### Mouse Input Issues

Do **not** multiply mouse input by `Time.deltaTime`.  

If you are using combined input with a joystick, only scale the joystick portion of this input.

#### Why?
Mouse input is a *delta*. The value is tracking change.  
Most input is multiplied by `Time.deltaTime`, this translates their fixed value, *"How far is the joystick tilted?"*, to a delta, *"How far should the character move this frame?"*.  
Seeing as mouse delta already describes how many units the mouse travelled this frame, it should not be scaled again. Doubly scaling the input will make the look speed frame rate dependent. If your game has hitches, you will feel it as jumpiness, as a longer frame-time will translate to more mouse travel, and more delta time.

---  

When using the new Input System, InputActions [accumulate this delta](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/index.html?subfolder=/api/UnityEngine.InputSystem.Pointer.html#UnityEngine_InputSystem_Pointer_delta) (see `delta`) in such a way that means values during this accumulation may not be what you expect. Using the last read value in an Update should give you an appropriate value for that frame. More complex sub-frame sampling may require more custom handling of this data with that in mind.