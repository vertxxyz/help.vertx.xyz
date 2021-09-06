### Variables with Custom Editors

#### Description
If a variable is only appearing in debug mode the _Component_ or _Scriptable Object_ you are inspecting must have a custom inspector, or the variable you're looking at may have a custom drawer.

#### Resolution
First, search your project for inspectors that match your _Component_ or _Scriptable Object_.
They are usually named with the same name as the script, except have "Editor" or "Inspector" appended.  

If you cannot find the Editor, then perhaps the type you are trying to serialize has a custom Property Drawer. They are usually named with the same name as the type, except have "Drawer" appended.  

Once you have found a script, you can either choose to edit it to include the variable, or remove the inspector entirely.
If you want to remove, make sure you have a backup before doing so.  