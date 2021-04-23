### Collision Matrix

If your two colliders in question do not match in the matrix then you **will not** get collision messages

+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|                               | Static | Rigidbody | Kinematic Rigidbody | Static Trigger | Rigidbody Trigger | Kinematic Rigidbody Trigger |
+===============================+========+===========+=====================+================+===================+=============================+
|**Static**                     |N       |Y          |N                    |N               |N                  |N                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Rigidbody**                  |Y       |Y          |Y                    |N               |N                  |N                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Kinematic Rigidbody**        |N       |Y          |N                    |N               |N                  |N                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Static Trigger**             |N       |N          |N                    |N               |N                  |N                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Rigidbody Trigger**          |N       |N          |N                    |N               |N                  |N                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Kinematic Rigidbody Trigger**|N       |N          |N                    |N               |N                  |N                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+

You can read more about this and see a copy of this matrix in the Unity docs [here](https://docs.unity3d.com/Manual/CollidersOverview.html).

:::info
If you are using a Character Controller you may be looking for the [OnControllerColliderHit](https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html) message
:::

---
[I am still not getting a message](4%20Local%20Functions%203D.md)