### Collision Action Matrix

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

### Trigger Action Matrix

If your two colliders in question do not match in the matrix then you **will not** get trigger messages

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

---

You can read more about this and see a copy of these matrices in the Unity docs [here](https://docs.unity3d.com/Manual/CollidersOverview.html).