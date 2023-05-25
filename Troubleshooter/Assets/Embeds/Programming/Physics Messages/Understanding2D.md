### Understanding this matrix:
Your two colliders are described by an element in a row and in a column, the intersection of these two shows whether this type of event will be fired.  
If the event is not green, consider changing your setup, or switching event type.


| Type                            | Rigidbody                                | Collider              |
|---------------------------------|------------------------------------------|-----------------------|
| **Static**                      | None,<br>or Body Type set to **Static**. | Is Trigger is off.    |
| **Rigidbody**                   | Body Type set to **Dynamic**.            | Is Trigger is off.    |
| **Kinematic Rigidbody**         | Body Type set to **Kinematic**.          | Is Trigger is off.    |
| **Static Trigger**              | None,<br>or Body Type set to **Static**. | Is Trigger is **on**. |
| **Rigidbody Trigger**           | Body Type set to **Dynamic**.            | Is Trigger is **on**. |
| **Kinematic Rigidbody Trigger** | Body Type set to **Kinematic**.          | Is Trigger is **on**. |

:::note{.inline}
A collider is required on both objects.
:::