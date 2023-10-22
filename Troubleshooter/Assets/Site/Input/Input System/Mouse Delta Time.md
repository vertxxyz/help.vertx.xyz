## Mouse input issues

<<Programming/Input/DeltaTime.md>>  

### Input System package: Details

When using the new Input System, InputActions [accumulate this delta](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/index.html?subfolder=/api/UnityEngine.InputSystem.Pointer.html#UnityEngine_InputSystem_Pointer_delta) (see `delta`) in such a way that means values during this accumulation may not be what you expect. Using the last read value in an Update should give you an appropriate value for that frame. More complex sub-frame sampling may require more custom handling of this data with that in mind.  

**Edit | Project Settings | Input System Package** has important settings for when input is processed.  
**Update Mode** should be set to Dynamic Update to process input at the rate of Update. If this is set to Fixed Update then input may not appear accurate at high frame rates.