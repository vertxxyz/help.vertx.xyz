# Common exceptions
Exceptions are a subset of runtime errors that can be cleared from the Console window.
When an exception is thrown execution redirected to the nearest `try`-`catch` block (often in Unity's code).
This means that code placed directly after an exception-raising statement will not be executed.

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
- [TypeLoadException](Runtime%20Exceptions/TypeLoadException.md)

:::

# Common errors
## General
:::code-block--no-background
- [BroadcastMessage X has no receiver!](Runtime%20Errors/BroadcastMessage.md)
- [Coroutine couldn't be started because the the game object 'X' is inactive!](Coroutines/Inactive%20Objects.md)
- [Destroying assets is not permitted to avoid data loss.](Runtime%20Errors/Destroying%20Assets.md)
- ['Example' is missing the class attribute 'ExtensionOfNativeClass'!](Script%20Loading%20Issues.md)
- [Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption.](Runtime%20Errors/Prefab%20Modifications_Parents.md)
- [You are trying to create a MonoBehaviour using the 'new' keyword.](Runtime%20Errors/MonoBehaviourNew.md)

:::

## Layers and tags
:::code-block--no-background  
- [A game object can only be in one layer. The layer needs to be in the range [0...31]](Runtime%20Errors/Undefined%20Layer.md)
- [Tag: Example is not defined.](Runtime%20Errors/Undefined%20Tag.md)

:::

## Animation
:::code-block--no-background  
- ['X' AnimationEvent 'Y' on animation 'Z' has no receiver! Are you missing a component?](Animation/Animation%20Event/Receivers.md)
- ['X' AnimationEvent has no function name specified!](Animation/Animation%20Event/Functions.md)

:::

## Physics
:::code-block--no-background  
- [Script error: OnCollisionEnter.](Physics%20Messages/2%20Collision%20Messages%203D.md)

:::  

## Other

- [Built-in input system errors.](Input/Built-In%20Input.md)
- [Input system package errors.](Input/Input%20System/Errors.md)
- [Burst compiler errors.](DOTS/Burst/Common%20Errors.md)
- [Entities errors.](DOTS/Entities/Common%20Errors.md)

## Editor
These errors may occur outside of Play Mode:  
:::code-block--no-background
- [Serialization depth limit exceeded at 'X'. There may be an object composition cycle in one or more of your serialized classes.](Runtime%20Errors/Serialization%20Depth%20Limit.md)

:::

These errors are only relevant to you if you are writing editor code:  
:::code-block--no-background
- [type is not a supported pptr value.](Runtime%20Errors/ObjectReferenceValue%20Error.md)

:::
