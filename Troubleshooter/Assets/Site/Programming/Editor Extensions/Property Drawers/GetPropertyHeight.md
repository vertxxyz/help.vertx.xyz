## PropertyDrawer: GetPropertyHeight

PropertyDrawers don't have a layout phase that causes them to expand to contain the drawn content.  
UI Toolkit property drawers don't have this restriction, but the editor will only use a UI Toolkit drawer if the editor containing it is also UI Toolkit.

### Resolution
Manually define the height by overriding the [GetPropertyHeight](https://docs.unity3d.com/ScriptReference/PropertyDrawer.GetPropertyHeight.html) function.