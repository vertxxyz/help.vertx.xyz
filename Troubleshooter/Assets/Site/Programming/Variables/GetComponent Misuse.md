## GetComponent misuse
### Description
When assigning variables via the Inspector there is no need to use `GetComponent` in code.  
[GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) also only gets Components on the same GameObject, otherwise it will return `null`.  
This explains why the variable becomes missing when playing the game.  

### Resolution
Remove the `GetComponent` call from Start, or Awake. This variable is assigned in the inspector instead.  

---  

If you want to assign a default variable when the component is added you can do this in the [Reset](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Reset.html) method.