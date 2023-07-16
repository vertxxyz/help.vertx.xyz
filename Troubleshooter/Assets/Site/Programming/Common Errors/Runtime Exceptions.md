## Common runtime exceptions
### Exceptions

:::code-block--no-background
- [NullReferenceException](Runtime%20Exceptions/NullReferenceException.md)
- [MissingReferenceException](Runtime%20Exceptions/MissingReferenceException.md)
- [UnassignedReferenceException](Runtime%20Exceptions/UnassignedReferenceException.md)
- [IndexOutOfRangeException](Runtime%20Exceptions/IndexOutOfRangeException.md)
- [ArgumentException](Runtime%20Exceptions/ArgumentException.md)
- [ArgumentNullException](Runtime%20Exceptions/ArgumentNullException.md)
- [InvalidCastException](Runtime%20Exceptions/InvalidCastException.md)
- [InvalidOperationException](Runtime%20Exceptions/InvalidOperationException.md)
- [StackOverflowException](Runtime%20Exceptions/StackOverflowException.md)

:::

### Errors
#### Runtime
:::code-block--no-background
- [You are trying to create a MonoBehaviour using the 'new' keyword.](Runtime%20Errors/MonoBehaviourNew.md)
- ['Example' is missing the class attribute 'ExtensionOfNativeClass'!](../Scripts/Loading%20Issues.md)
- [Tag: Example is not defined.](Runtime%20Errors/Undefined%20Tag.md)
- [A game object can only be in one layer. The layer needs to be in the range [0...31]](Runtime%20Errors/Undefined%20Layer.md)
- ['X' AnimationEvent 'Y' on animation 'Z' has no receiver! Are you missing a component?](../../Animation/Animation%20Event/Receivers.md)
- ['X' AnimationEvent has no function name specified!](../../Animation/Animation%20Event/Functions.md)
- [BroadcastMessage X has no receiver!](Runtime%20Errors/BroadcastMessage.md)
- [Destroying assets is not permitted to avoid data loss.](Runtime%20Errors/Destroying%20Assets.md)
- [Coroutine couldn't be started because the the game object 'X' is inactive!](../Coroutines/Inactive%20Objects.md)
- [Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption.](Runtime%20Errors/Prefab%20Modifications_Parents.md)
- [Script error: OnCollisionEnter.](../Physics%20Messages/2%20Collision%20Messages%202D.md)

:::  

---
- [Built-in input system errors.](../Input/Built-In%20Input.md)  
- [Input system package errors.](../Input/Input%20System/Errors.md)
- [Burst compiler errors.](../../DOTS/Burst/Common%20Errors.md)
- [Entities errors.](../../DOTS/Entities/Common%20Errors.md)
 
#### Editor
These errors occur outside of Play mode:  
:::code-block--no-background

- [Serialization depth limit exceeded at 'X'. There may be an object composition cycle in one or more of your serialized classes.](Runtime%20Errors/Serialization%20Depth%20Limit.md)

:::

These errors are only relevant to you if you are writing editor code:  
:::code-block--no-background
- [type is not a supported pptr value.](Runtime%20Errors/ObjectReferenceValue%20Error.md)

:::