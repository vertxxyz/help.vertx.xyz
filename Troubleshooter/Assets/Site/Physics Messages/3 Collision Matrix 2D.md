## Collision matrix (2D)

If your two colliders don't match in the matrix you **will not** get collision messages.  
If you're trying to receive events that don't match, consider using a [trigger](3%20Trigger%20Matrix%202D.md) event instead, note that they have a [different method signature](2%20Trigger%20Messages%202D.md).


^^^
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
^^^ You can read more about this and see a copy of this matrix in the Unity docs [here](https://docs.unity3d.com/Manual/CollidersOverview.html).

<<Programming/Physics Messages/Understanding2D.md>>

---
[I am still not getting a message.](4%20Local%20Functions%202D.md)