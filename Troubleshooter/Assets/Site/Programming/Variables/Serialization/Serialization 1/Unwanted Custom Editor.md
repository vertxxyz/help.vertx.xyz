## Variables with Custom Editors

### Description
If a variable is only appearing in debug mode the Component or Scriptable Object you are inspecting must have a custom inspector, or the variable you're looking at may have a custom drawer.

### Resolution
First, search the project for inspectors (**Editor**) that target the problematic Component or Scriptable Object.
They are usually named with the same name as the script, except have "Editor" or "Inspector" appended.  

If the Editor cannot be found, then perhaps the type you are trying to serialize has a custom **Property Drawer**. They are usually named with the same name as the type, except have "Drawer" appended.  

Once a script has been found, you can either choose to edit it to include the variable, or remove the inspector entirely.
If removal is chosen, make sure a backup is made before doing so.  