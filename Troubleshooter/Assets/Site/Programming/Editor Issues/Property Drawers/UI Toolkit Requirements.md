## Property drawers: UI Toolkit requirements

As of Unity 2022.2, UI Toolkit is the default backend for the inspector, before then the Editor was implemented using IMGUI.

### Resolution

#### Unity 2022.2 and above

Check your editor is being drawn with UI Toolkit by using the [UI Toolkit Debugger](https://docs.unity3d.com/Manual/UIE-ui-debugger.html) (**Window | UI Toolkit | Debugger**).  

:::note  
#### The editor is drawn with IMGUI
There are custom packages (Naughty Attributes for example) that override all editors for MonoBehaviour and ScriptableObject (or UnityEngine.Object) subtypes. If a package like this is present in the project you will have to resort to creating a UI Toolkit [Editor](https://docs.unity3d.com/ScriptReference/Editor.html) for the components that draw this property.
If you are trying to find these Editors in your project you can search for `[CustomEditor(typeof(Object)`, `[CustomEditor(typeof(MonoBehaviour)`, `[CustomEditor(typeof(ScriptableObject)`, until you find the overriding editor.  
:::

:::note  
#### The editor is drawn with UI Toolkit
You mustn't have properly implemented the [`CreatePropertyGUI`](https://docs.unity3d.com/ScriptReference/PropertyDrawer.CreatePropertyGUI.html) method.  
:::

#### Before Unity 2022.2

UI Toolkit property drawers cannot be drawn inside of an IMGUI context. Create a UI Toolkit [Editor](https://docs.unity3d.com/ScriptReference/Editor.html) for the components that draw this property.  

---

Optionally create a IMGUI fallback for the property drawer. This will allow your field to be drawn in IMGUI contexts. As IMGUI drawers can be displayed inside UI Toolkit contexts, you could _just_ write an IMGUI drawer, though this will be less performant.