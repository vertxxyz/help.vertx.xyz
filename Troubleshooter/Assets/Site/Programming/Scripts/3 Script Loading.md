## Base Type
### Description
Components and ScriptableObject scripts must inherit from `MonoBehaviour` and `ScriptableObject`.  
If a user type does not inherit from `MonoBehaviour` it cannot be attached to a GameObject.  
If a user type does not inherit from `ScriptableObject` it cannot be instanced as a scriptable asset.  

### Resolution
Ensure your class appropriately inherits from `MonoBehaviour` or `ScriptableObject`.  

<<Code/Scripts/Script Loading 1.rtf>>  