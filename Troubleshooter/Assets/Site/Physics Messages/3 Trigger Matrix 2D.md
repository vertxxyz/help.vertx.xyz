# Trigger matrix (2D)

If your two colliders don't match in the matrix you **will not** get trigger messages.  
If you're trying to receive events that don't match, consider using a [collision](3%20Collision%20Matrix%202D.md) event instead, note that they have a [different method signature](2%20Collision%20Messages%202D.md).

^^^
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|                               | Static | Rigidbody | Kinematic Rigidbody | Static Trigger | Rigidbody Trigger | Kinematic Rigidbody Trigger |
+===============================+========+===========+=====================+================+===================+=============================+
|**Static**                     |N       |N          |N                    |N               |Y                  |Y                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Rigidbody**                  |N       |N          |N                    |Y               |Y                  |Y                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Kinematic Rigidbody**        |N       |N          |N                    |Y               |Y                  |Y                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Static Trigger**             |N       |Y          |Y                    |N               |Y                  |Y                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Rigidbody Trigger**          |Y       |Y          |Y                    |Y               |Y                  |Y                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
|**Kinematic Rigidbody Trigger**|Y       |Y          |Y                    |Y               |Y                  |Y                            |
+-------------------------------+--------+-----------+---------------------+----------------+-------------------+-----------------------------+
^^^ You can read more about this and see a copy of this matrix in the Unity docs [here](https://docs.unity3d.com/Manual/CollidersOverview.html).

<<Programming/Physics Messages/Understanding2D.md>>

---
[I am still not getting a message.](4%20Local%20Functions%202D.md)
