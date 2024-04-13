# UI Toolkit: Transitions

If you are having transitions occur instantaneously, consider the following things:

1. Increase the transition's duration to a reasonable amount of time to check that it is actually working.  
   It's extremely important to note whether your transition is occuring in `ms` (milliseconds) or `s` (seconds).
1. Add the transition to a style that is present during the entire duration of your transition.  
   If a style is removed as its specified transition is occuring, it will be cancelled.
1. If an element is never evaluated in a state, that state will never form a point in the transition.  
   For example, creating an element (or enabling a document) and adding a USS class in a single frame will not perform a transition. Consider adding the class [after a delay](#creating-delays).
1. The target property must be marked as [fully animatable](https://docs.unity3d.com/Manual/UIE-USS-Properties-Reference.html).

## Creating delays
You can use a VisualElement's [`schedule`](https://docs.unity3d.com/ScriptReference/UIElements.VisualElement-schedule.html) property to retrieve its [`IVisualElementScheduler`](https://docs.unity3d.com/ScriptReference/UIElements.IVisualElementScheduler.html), which can execute actions later.

```csharp
// Adds a class to an element on the next repaint.
element.schedule.Execute(() => element.AddToClassList(exampleUssClassName));
```

You can chain further calls to the Execute function to add more specific scheduling.
```csharp
// Example that executes something after 200ms.
.Execute(...).StartingIn(200);
```

---

If you find an unlisted resolution please <<report-issue.html>> so this page can be improved.  
