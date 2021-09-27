## Property Drawers - UIToolkit Requirements
### Description
By default the Editor that draws fields is an IMGUI inspector.  
UIToolkit property drawers cannot be drawn inside of an IMGUI context.

### Resolution
Create a UIToolkit [Editor](https://docs.unity3d.com/ScriptReference/Editor.html) for the components that draw this property.  
Optionally, but advisable, create a IMGUI fallback for the property drawer. This will allow your field to be drawn in IMGUI contexts.  
As IMGUI property drawers can be drawn inside UIToolkit contexts, you could also solely write an IMGUI drawer, though it will be less performant.