
Do **not** multiply mouse input by `Time.deltaTime`.

If you are using combined input with a joystick, only scale the joystick portion of this input.

### Why?
Mouse input is a *delta*. The value is tracking change.  
Most input is multiplied by `Time.deltaTime`, this translates their fixed value, *"How far is the joystick tilted?"*, to a delta, *"How far should the character move this frame?"*.  
Seeing as mouse delta already describes how many units the mouse travelled this frame, it should not be scaled again. Doubly scaling the input will make the look speed frame rate dependent. If your game has hitches, you will feel it as jumpiness, as a longer frame-time will translate to more mouse travel, and more delta time.