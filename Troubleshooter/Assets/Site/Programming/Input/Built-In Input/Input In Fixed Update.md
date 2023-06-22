## Input in Fixed Update
### Description
**Instantaneous** [`Input`](https://docs.unity3d.com/ScriptReference/Input.html) functions ([`GetKeyDown`](https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html), [`GetMouseButtonDown`](https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html), and others) will trigger inconsistently when used in `FixedUpdate` or any physics message function like `OnCollisionEnter`.  

#### Why?
Physics is run at a fixed rate, with physics sometimes  updating multiple times a frame or not at all. If input occurs on a frame where `FixedUpdate` isn't run, it won't be processed by your code.  
See [`FixedUpdate`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html) for more information.  

### Resolution

:::note  
#### I am using FixedUpdate
Cache values in [`Update`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html), and consume them in `FixedUpdate`.

#### Example
```csharp
private bool _jump;

void Update()
{
    // If jump wasn't consumed we stay true, even when jump wasn't pressed this frame.
    _jump |= Input.GetKeyDown(KeyCode.Space);
}

void FixedUpdate()
{
    if (_jump)
    {
        rigidbody.AddForce(_jumpForce, ForceMode.Impulse);
        // Consume the jump input:
        _jump = false;
    }
}
```
:::  

:::note
#### I am using a physics message callback
When using physics callbacks and not calling physics functions, you may be able to move your logic to `Update`.

#### Example

```csharp
private bool _triggerIsOccupied;

void Update()
{
    if (!_triggerIsOccupied)
    {
        // Exit early, trigger is not occupied.
        return;
    }

    // Handle input in Update:
    if (Input.GetKeyDown(KeyCode.Space))
    {
        Example();
    }
}

/* Keep track of whether the trigger is occupied: */

void OnTriggerEnter(Collider other)
{
    _triggerIsOccupied = true;
}

void OnTriggerExit(Collider other)
{
    _triggerIsOccupied = false;
}
```

If you expect overlapping triggers, you can use a counter instead of a boolean.  
:::  