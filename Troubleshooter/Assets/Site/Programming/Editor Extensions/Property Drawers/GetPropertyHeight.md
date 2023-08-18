## PropertyDrawer: GetPropertyHeight

Property Drawers don't have a layout phase that causes them to expand to contain the drawn content.  
UI Toolkit property drawers don't have this restriction, but the editor will only use a UI Toolkit drawer if the editor containing it is also UI Toolkit.

### Resolution
#### This occurs every time the Property Drawer is used
Manually define the height by overriding the [`GetPropertyHeight`](https://docs.unity3d.com/ScriptReference/PropertyDrawer.GetPropertyHeight.html) function.

#### This only occurs for the first element of a collection
This was a bug that has been fixed in later Unity versions, update Unity to the latest patch version to get the fix.

> Editor: Fixed ReorderableList first element height when the element uses PropertyDrawer. ([1409773](https://issuetracker.unity3d.com/issues/first-array-element-expansion-is-broken-for-arrays-that-use-custom-property-drawers))