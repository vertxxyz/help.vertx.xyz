## Player logs
Player-related log locations
To view the Player log, open a Console Window (**Window | General | Console**) and select **Open Player Log** from the Console window menu. You can also navigate to the following folder:

| Operating system           | Player log location                                                                                                                                    |
|----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| Android                    | To access the Player log for an Android application, use Android logcat. For more information, see View Android logs.                                  |
| iOS                        | Use the GDB console, or the Organizer Console through XCode to access iOS device logs. For more information on device logs, see Apple’s documentation. |
| Linux                      | `~/.config/unity3d/CompanyName/ProductName/Player.log`                                                                                                 |
| macOS                      | `~/Library/Logs/Company Name/Product Name/Player.log`<br/>Note: You can also use the Console.app utility to find the log file.                         |
| Universal Windows Platform | `%USERPROFILE%\AppData\Local\Packages\<productname>\TempState\UnityPlayer.log`                                                                         |
| WebGL                      | Unity writes the log output to your browser’s JavaScript console.                                                                                      |
| Windows                    | `%USERPROFILE%\AppData\LocalLow\CompanyName\ProductName\Player.log`                                                                                    |

You can see this table on the [Unity log files documentation](https://docs.unity3d.com/Manual/LogFiles.html).