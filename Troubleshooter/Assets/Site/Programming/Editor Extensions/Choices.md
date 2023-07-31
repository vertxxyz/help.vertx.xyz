## Choosing a type of editor extension

```mermaid
%%{ init: { 'flowchart': { 'curve': 'linear' } } }%%
flowchart TB
    Start[Do you want to create custom UI or a command?]-->|Custom UI| IsDecorative
    IsDecorative(Is the UI decorative,\nand doesn't read/write property values?)--->|Yes| Decorator([Create a Property Decorator])
    IsDecorative--->|No| SingleField(Is the UI modifying a single field?)
    SingleField--->|Yes| Drawer([Create a Property Drawer])
    SingleField--->|No| Subset(Is the UI modifying a grouped set of fields?)
    Subset--->|Yes| Drawer
    Subset--->|No| NoObject(Is the UI modifying specific selected objects?)
    NoObject--->|Yes| ComplexLogic(Does the UI require complex interconnected logic?)
    NoObject--->|No| EditorWindow
    ComplexLogic-->|No| Drawer
    ComplexLogic-->|Yes| Inspector([Create a Custom Editor])


    Start-->|Custom command| HasSettings
    HasSettings(Does the command require user-modified custom settings?)--->|Yes| EditorWindow([Create an Editor Window])
    HasSettings--->|No| IsObjects(Does the command modify a single object at a time?)
    IsObjects--->|Yes| ContextMenu([Create a Context Menu])
    IsObjects--->|No| IsSameType(Does the command target a single type of Object?)
    IsSameType--->|Yes| ContextMenu
    IsSameType--->|No| MenuItem([Create a Menu Item])

    click Drawer href "https://docs.unity3d.com/ScriptReference/PropertyDrawer.html" "PropertyDrawer documentation"
    click MenuItem href "https://docs.unity3d.com/ScriptReference/MenuItem.html" "MenuItem documentation"
    click ContextMenu href "https://docs.unity3d.com/ScriptReference/ContextMenu.html" "ContextMenu documentation"
    click EditorWindow href "https://docs.unity3d.com/ScriptReference/EditorWindow.html" "EditorWindow documentation"
    click Decorator href "https://docs.unity3d.com/ScriptReference/DecoratorDrawer.html" "DecoratorDrawer documentation"
    click Decorator href "https://docs.unity3d.com/ScriptReference/Editor.html" "Editor documentation"
```