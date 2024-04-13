# Unity Hub: Editor installation issues
## Make space on your primary drive
You need space on your primary (C:) drive as the Unity Hub will cache downloads there during installations.

Modern versions of the Hub allow you to change these locations (**Preferences ⚙️ | Installs**), but space on the primary drive is still necessary for certain components.

## Antivirus
Check that the Hub is not being blocked by antivirus/security programs, and attempt to whitelist the Hub.

## Logging locations
Check the logs, looking for setup issues close to the bottom of the output:

^^^

| Operating system | Unity Hub log location                        |
|------------------|-----------------------------------------------|
| Windows          | `%UserProfile%\AppData\Roaming\UnityHub\logs` |
| macOS            | `~/Library/Application support/UnityHub/logs` |
| Linux            | `~/.config/UnityHub/logs`                     |
^^^ You can see this table in the [documentation](https://docs.unity3d.com/Manual/LogFiles.html)

You can also get to this location via the account dropdown in the top left of the Hub **▾ | Troubleshooting | Open log folder**.

## Different installation locations
Try installing into a different location using the settings (**Preferences ⚙️ | Installs | Installs Location**).

If you are using macOS, you should check that the Unity Hub has write permissions in the **Privacy and Security | Files and Folders** preferences for the folder you are using (if present).

## Permissions
Don't launch the Hub with Administrator privileges as it may prompt you to avoid doing so. Proper configuration and application permissions should be applied normally.

## Restart
1. Restart your computer.
1. When the Hub next opens, install any updates it may have.


## Ask about it on the forums
**Check the pinned messages** 📌, and search the [Unity Hub forums](https://forum.unity.com/forums/unity-hub.142/) for your issue, and then make a post there if you cannot find a resolution.

---
If you find an unlisted resolution please <<report-issue.html>> so this page can be improved.  
