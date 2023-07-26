### Understanding this matrix:
Your two colliders are described by an element in a row and in a column, the intersection of these two shows whether this type of event will be fired.  
If the event is not green, consider changing your setup, or switching event type.

A [Collider](https://docs.unity3d.com/Manual/collision-section.html) is required on both objects, and at least one object needs a [Rigidbody](https://docs.unity3d.com/Manual/class-Rigidbody.html). Their settings are described below:


| Type                        | Rigidbody               | Collider              |
|-----------------------------|-------------------------|-----------------------|
| Static                      | None.                   | Is Trigger is off.    |
| Rigidbody                   | Is Kinematic is off.    | Is Trigger is off.    |
| Kinematic Rigidbody         | Is Kinematic is **on**. | Is Trigger is off.    |
| Static Trigger              | None.                   | Is Trigger is **on**. |
| Rigidbody Trigger           | Is Kinematic is off.    | Is Trigger is **on**. |
| Kinematic Rigidbody Trigger | Is Kinematic is **on**. | Is Trigger is **on**. |