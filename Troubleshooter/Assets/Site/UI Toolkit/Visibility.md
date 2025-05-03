# UI Toolkit visibility
Visibility `hidden` isn't inherited. This means that if a child is set to `visible`, it will be shown.

## Resolution
### In code
- Avoid using `Visibility.Visible` and instead set to `StyleKeyword.Null`.
- Avoid the [`visibile`](https://docs.unity3d.com/ScriptReference/UIElements.VisualElement-visible.html) property, instead create an extension method that uses corrected behaviour.

```csharp
/// <summary>
/// Styles an element to stop it from rendering.
/// </summary>
/// <param name="element">The targeted element.</param>
/// <param name="isHidden"><paramref name="element"/> is hidden if true.</param>
/// <remarks>Unlike <see cref="VisualElement.visible"/>, because <see cref="Visibility.Visible"/> <i>forces</i> visibility (even as a child of a hidden element)
/// this method sets the visibility to <see cref="StyleKeyword.Null"/>.</remarks>
public static void SetHidden(this VisualElement element, bool isHidden) => element.style.visibility = isHidden ? Visibility.Hidden : StyleKeyword.Null;
```

### In USS
Avoid the `visibile` value; instead try to invert the logic and only apply `hidden`.  
Note that [USS variables](https://docs.unity3d.com/Manual/UIE-USS-CustomProperties.html) can help solve tricky problems and avoid complex selectors.
