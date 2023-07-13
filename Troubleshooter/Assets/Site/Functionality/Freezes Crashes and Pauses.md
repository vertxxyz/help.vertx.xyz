## Freezes, halts, and crashes
### Freezing
Freezing is where a program does not close, but cannot be interacted with.  
- [My game freezes unexpectedly.](Freezing.md)
- [My game freezes when I hit a breakpoint.](../Programming/Debugging/Debugger.md#breakpoints)
### Crashing
Crashing is when a program closes unexpectedly.  

Check the appropriate logs ([documentation](https://docs.unity3d.com/Manual/LogFiles.html)), looking for possible causes at the bottom.

#### Unity Editor
- Windows: `%LOCALAPPDATA%\Unity\Editor\Editor.log`
- MacOS: `~/Library/Logs/Unity/Editor.log`
- Linux: `~/.config/unity3d/Editor.log`

#### Unity Hub
- Windows: `%UserProfile%\AppData\Roaming\UnityHub\logs`  
- Mac: `~/Library/Application support/UnityHub/logs`  
- Linux: `~/.config/UnityHub/logs`  

### Pausing
Pausing is when the application halts momentarily.  
:::note  
🚧 Under Construction 🚧  
:::

- [Unity **pauses the Editor** unexpectedly.](Play%20Mode/Error%20Pause.md)