## Animation Event receivers
Animation Events are callbacks that fire during an animation's playback.  
These events require a component with the correct method signature to be attached to the GameObject with the Animator.

### Resolution
#### Adding a valid receiver
1. Add a component to the GameObject with the Animator on it.
1. Make a public method matching the name of the event. The method must not have a return value.
1. The method's parameter must match the event. Animation Events can have `float`, `int`, `string`, `Object`, `AnimationEvent`, or no parameters.

#### Removing the Animation Event
If the event is no longer required, it can be removed.  
1. Open the [Animation window](https://docs.unity3d.com/Manual/animeditor-UsingAnimationEditor.html) (**Window | Animation**).
1. Select the Animation Clip mentioned in the error.
1. In the [Event Line](https://docs.unity3d.com/Manual/script-AnimationWindowEvent.html), select the Animation Event.
1. Delete the event.

If the Animation window is showing a warning when your clip is selected:
1. Select the model that the clip was created from in the project.
1. Select the Animation tab and the clip mentioned in the error.
1. Expand the Events dropdown.
1. Select the event and delete it.