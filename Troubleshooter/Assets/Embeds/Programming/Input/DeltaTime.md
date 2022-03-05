
Don't multiply mouse input by `Time.deltaTime`.

If you are using combined input with a joystick, only scale the joystick portion of this input.  

:::info{.inline}
You will need to reduce the sensitivity to compensate for this.
:::  

### Why?
Mouse input is a *delta*. The value is tracking change.  
Most input is multiplied by `Time.deltaTime`, this translates their fixed value, *"How far is the joystick tilted?"*, to a delta, *"How far should the character move this frame?"*.  
Seeing as mouse delta already describes how many units the mouse travelled this frame, it should not be scaled again. Doubly scaling the input will make the look speed frame rate dependent. If your game has hitches, you will feel it as jumpiness, as a longer frame-time will translate to more mouse travel, and more delta time.

### Jitter with moving rigidbodies

[This article](https://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8) details the appearance of jitter in relation to the movement of physics bodies (which can include the player character).  
Generally, applying interpolation to rigidbodies that require a level of visual accuracy is advised.