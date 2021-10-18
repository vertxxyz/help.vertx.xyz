## Property drawers - UI Toolkit requirements
### Description
By default the Editor that draws fields is an IMGUI inspector.  
UI Toolkit property drawers cannot be drawn inside of an IMGUI context.

### Resolution
Create a UI Toolkit [Editor](https://docs.unity3d.com/ScriptReference/Editor.html) for the components that draw this property.  
Optionally, but advisable, create a IMGUI fallback for the property drawer. This will allow your field to be drawn in IMGUI contexts.  
As IMGUI property drawers can be drawn inside UI Toolkit contexts, you could also solely write an IMGUI drawer, though it will be less performant.