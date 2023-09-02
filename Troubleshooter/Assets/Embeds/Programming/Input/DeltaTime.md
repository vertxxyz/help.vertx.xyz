
:::error  
Don't scale mouse input by [`Time.deltaTime`](https://docs.unity3d.com/ScriptReference/Time-deltaTime.html)!  
:::

If you are using combined input with a joystick, only scale the joystick portion of this input.  

:::info{.small}
You will need to reduce the sensitivity to compensate for this change.
:::  

### Why?
Mouse input is already a delta (a rate of change).  
> How many units the mouse travelled this frame

It is already dependent on the time between frames.

DeltaTime scaling will make the value **frame rate dependent**. If your game has hitches, you will feel it as jumpiness, as a longer frame-time will give you more time to move your mouse, which you then incorrectly scale again with a larger `deltaTime` value.


#### When to use deltaTime:
Most input is multiplied by `Time.deltaTime`, this translates their **fixed value**:
> How far is the joystick tilted?  

To a **delta**:
> How far should the character move this frame?  

Anything that returns a fixed value when evaluated should be scaled by deltaTime. For example, joysticks, held key presses (like WSAD movement), and constants.  
If the returned value is dependent on the length of a frame, or isn't constantly evaluated, then no scaling should be applied. For example, mouse delta, scroll-wheel delta, and button presses.

### Comparing approaches
:::note  
#### Scenario 1: You move your mouse 100px over a 20ms frame and 200px over a 40ms frame
##### 🟢 Not scaling by deltaTime

| Frame | Frame time | Movement | Added | Total   |
|-------|------------|----------|-------|---------|
| 1     | 0.02ms     | 100      | 100   | 100     |
| 2     | 0.04ms     | 200      | 200   | **300** |

The value we added on the frame 2 was twice as large as frame 1, this makes sense 🙂  

##### 🔴 Scaling by deltaTime

| Frame | Frame time | Movement | Added | Total  |
|-------|------------|----------|-------|--------|
| 1     | 0.02ms     | 100      | 2     | 2      |
| 2     | 0.04ms     | 200      | 8     | **10** |

The value we added on frame 2 was four times as large as frame 1, this makes no sense ☹️
:::  
:::note  
#### Scenario 2: You move mouse 200px over a 20ms frame and 100px over a 40ms frame
##### 🟢 Not scaling by deltaTime

| Frame | Frame time | Movement | Added | Total   |
|-------|------------|----------|-------|---------|
| 1     | 0.02ms     | 200      | 200   | 200     |
| 2     | 0.04ms     | 100      | 100   | **300** |

The total is the same as the previous scenario, this makes sense 🙂

##### 🔴 Scaling by deltaTime

| Frame | Frame time | Movement | Added | Total |
|-------|------------|----------|-------|-------|
| 1     | 0.02ms     | 200      | 4     | 4     |
| 2     | 0.04ms     | 100      | 4     | **8** |

The total is different to the previous scenario, this makes no sense ☹️

:::  

### Jitter with moving rigidbodies

[This article](https://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8) details the appearance of jitter in relation to the movement of physics bodies (which can include the player character).  
Generally, applying interpolation to rigidbodies that require a level of visual accuracy is advised.