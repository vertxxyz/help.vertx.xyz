# UI Toolkit Live Reload doesn't work
If Live Reload doesn't work during playmode, set **Edit | Preferences | Asset Pipeline | Auto Refresh** to **Enabled**.

Live Reload for UXML files requires the **UI Toolkit Live Reload** checkbox to be enabled for the relevant tab. Right-click on the header of the tab you're working in and check the setting. USS reloading doesn't require this setting.

## Notes on implementation
Live Reload for UXML files requires a complete rebuild of the UI. UI Documents have their visual tree unloaded and rebuilt, and `OnEnable` is called on all components attached to the gameobject.

For Live Reload to work well, you will have to cache the state of the UI and use it to return to the previous configuration.
