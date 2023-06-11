## Assignment issues with serialized fields
### Common mistakes
:::note  
#### 1. Mismatched types
You may be using a different type than is being assigned.
- Using `Text` when using TextMeshPro objects. Use `TextMeshProUGUI` or `TMP_Text` instead.  
  `Text` refers to the legacy UI text.
- Using a type under an incorrect namespace. Hover over the type in your IDE and ensure it has the namespace you expect.

:::  
:::note
#### 2. References between invalid locations
See the [valid references diagram](Valid%20References.md) to understand what locations can reference each other.  
If you're unsure how to reference one location from another, refer to the [how-to](Serialized%20References.md#how-to) section.  
:::  
:::note
#### 3. Attempting to assign to default references
If you have a script selected you are not looking at the correct location to assign references.  
Select a GameObject or asset (such as a prefab or scriptable object) and assign the [reference to an instance](Serializing%20Component%20References.md#reference-the-instance-in-the-inspector).

:::