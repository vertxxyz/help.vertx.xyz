## Field alignment in UI Toolkit
Elements in the inspector must use the `alignedFieldUssClassName` class (a field on any class inheriting from `BaseField<T>`) to receive the styling that aligns their labels to the same depth.

Add the class to the root VisualElement field that contains your label and control.

```csharp
public override VisualElement CreatePropertyGUI(SerializedProperty property)
{
    LayerField layerField = new LayerField(property.displayName) { bindingPath = property.propertyPath };
    layerField.AddToClassList(AlignedFieldUssClassName);
    return layerField;
}

// Use the implementation that works for your version.
// Any type that inherits from BaseField<T> can be used.
public static readonly string AlignedFieldUssClassName =
#if UNITY_2022_2_OR_NEWER
    BaseField<int>.alignedFieldUssClassName;
#else
    BaseField<int>.ussClassName + "__aligned";
#endif
```