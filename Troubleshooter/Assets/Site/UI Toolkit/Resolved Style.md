# UI Toolkit: Resolved style

UI Toolkit resolves styles asynchronously.  
[`resolvedStyle`](https://docs.unity3d.com/ScriptReference/UIElements.VisualElement-resolvedStyle.html), [`layout`](https://docs.unity3d.com/ScriptReference/UIElements.VisualElement-layout.html), [`contentRect`](https://docs.unity3d.com/ScriptReference/UIElements.VisualElement-contentRect.html), [`paddingRect`](https://docs.unity3d.com/ScriptReference/UIElements.VisualElement-paddingRect.html), and others, will return invalid values before it has been initialised.  

## Resolution
These members are initialised after an element is created in the hierarchy.  
You can use [`GeometryChangedEvent`](https://docs.unity3d.com/ScriptReference/UIElements.GeometryChangedEvent.html) or [`AttachToPanelEvent`](https://docs.unity3d.com/ScriptReference/UIElements.AttachToPanelEvent.html) to read from these values when they are valid.


```csharp
_visualElement.RegisterCallback<GeometryChangedEvent>(
    _ =>
    {
        // _visualElement.resolvedStyle will contain initialised values.
    }
);
```
