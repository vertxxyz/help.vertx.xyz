
:::warning  
Don't scale mouse input by [`Time.deltaTime`](https://docs.unity3d.com/ScriptReference/Time-deltaTime.html)!  
:::

If you are using combined input with a joystick, only scale the joystick portion of this input.  

:::info{.inline}
You will need to reduce the sensitivity to compensate for this change.
:::  

### Why?
Mouse input is already a **delta** (a rate of change).  

#### When to use deltaTime:
**Most** input is multiplied by `Time.deltaTime`, this translates their **fixed value**:
> How far is the joystick tilted?  

To a **delta**:

> How far should the character move this frame?  

#### Why mouse input should not be scaled:
Mouse delta already describes "how many units the mouse travelled this frame", it should not be scaled again.  
Doubly-scaling the input will make the value **frame rate dependent**. If your game has hitches, you will feel it as jumpiness, as a longer frame-time will give you more time to move your mouse, which you then incorrectly scale again with a larger `deltaTime` value.

### Jitter with moving rigidbodies

[This article](https://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8) details the appearance of jitter in relation to the movement of physics bodies (which can include the player character).  
Generally, applying interpolation to rigidbodies that require a level of visual accuracy is advised.