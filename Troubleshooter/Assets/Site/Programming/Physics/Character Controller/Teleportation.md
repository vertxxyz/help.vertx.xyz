## Character Controller Teleportation
To teleport a CharacterController you need to disable it.  
<<Code/Physics/CharacterController Teleport.rtf>>  

If you are still having issues teleporting a character controller try to call [Physics.SyncTransforms](https://docs.unity3d.com/ScriptReference/Physics.SyncTransforms.html) to apply Transform changes after you teleport the controller.