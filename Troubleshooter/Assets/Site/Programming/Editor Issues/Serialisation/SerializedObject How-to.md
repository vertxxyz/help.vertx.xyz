## SerializedObject: How-to
### Description
[SerializedObject](https://docs.unity3d.com/ScriptReference/SerializedObject.html) is a window into Unity's serialized datastructures. It exposes a [SerializedProperty](https://docs.unity3d.com/ScriptReference/SerializedProperty.html) structure, objects used to access items in the serialization hierarchy.  
SerializedObject and SerializedProperty are the best way to access and modify Unity-serialized structures; with automatic undo support, multi-object editing, and simplified functions for Editor UI.  

This guide only attempts to communicate how to access values, and not how to write entire editors. For the sake of linearity these examples use [IMGUI](https://docs.unity3d.com/Manual/GUIScriptingGuide.html), but I would recommend using [UI Toolkit](https://docs.unity3d.com/Manual/UIElements.html), which also supports UI binding by path name.

### Example structure

<<Code/Editor/SerializedObject/Base.html>>  

```nomnoml
<<Nomnoml/shared.nomnoml>>

[ScriptableObject (Example)|targetObject: UnityEngine.Object|UpdateIfRequiredOrScript()
ApplyModifiedProperties()
...]
[<label>FindProperty("data")]
[<label>FindProperty("values")]
[ScriptableObject (Example)]-[FindProperty("data")]
[FindProperty("data")]->[SerializedProperty (data)]
[ScriptableObject (Example)]-[FindProperty("values")]
[FindProperty("values")]->[SerializedProperty (values)]

[SerializedProperty (data)|]
[<label>FindPropertyRelative("Active")]
[<label>FindPropertyRelative("Configuration")]
[SerializedProperty (data)]-[FindPropertyRelative("Active")]
[FindPropertyRelative("Active")]->[SerializedProperty (Active)]
[SerializedProperty (data)]-[FindPropertyRelative("Configuration")]
[FindPropertyRelative("Configuration")]->[SerializedProperty (Configuration)]

[SerializedProperty (values)|arraySize: int]
[<label>GetArrayElementAtIndex(i)]
[SerializedProperty (values)]-[GetArrayElementAtIndex(i)]
[GetArrayElementAtIndex(i)]->[SerializedProperty (values\[0..arraySize\])]
[SerializedProperty (values\[0..arraySize\])|floatValue: float]

[SerializedProperty (Active)|boolValue: bool]
[SerializedProperty (Configuration)|objectReferenceValue: UnityEngine.Object]
```

### SerializedObject in Editors

```csharp
[CustomEditor(typeof(Example))]
public class ExampleInspector : Editor
{
    // This is the relevant scope
}
```

[Editor](https://docs.unity3d.com/ScriptReference/Editor.html) provides the [`serializedObject`](https://docs.unity3d.com/ScriptReference/Editor-serializedObject.html) of the object(s) it is inspecting. In our case this is an instance of `Example`.  
We can use [`FindProperty`](https://docs.unity3d.com/ScriptReference/SerializedObject.FindProperty.html) to get root SerializedProperties from the SerializedObject. For example:  

```csharp
private SerializedProperty data, values;

private void OnEnable()
{
    data = serializedObject.FindProperty("data");
    values = serializedObject.FindProperty("values");
}
```

```nomnoml
<<Nomnoml/shared.nomnoml>>
[ScriptableObject (Example)|]
[<label>FindProperty("data")]
[<label>FindProperty("values")]
[SerializedProperty (data)|]
[SerializedProperty (values)|]
[ScriptableObject (Example)]-[FindProperty("data")]
[FindProperty("data")]->[SerializedProperty (data)]
[ScriptableObject (Example)]-[FindProperty("values")]
[FindProperty("values")]->[SerializedProperty (values)]
```

Once we have valid SerializedProperties we can simply draw them using a [PropertyField](https://docs.unity3d.com/ScriptReference/EditorGUILayout.PropertyField.html).

```csharp
public override void OnInspectorGUI()
{
    EditorGUILayout.PropertyField(data);
    EditorGUILayout.PropertyField(values);
}
```

Changes to these values will not persist when edited as there's a small amount of work to be done to apply modifications:  

```csharp
public override void OnInspectorGUI()
{
    // Update the serializedObject to match the internal state if required.
    serializedObject.UpdateIfRequiredOrScript();
    
    EditorGUILayout.PropertyField(data);
    EditorGUILayout.PropertyField(values);
    
    // Apply changes made before this call.
    serializedObject.ApplyModifiedProperties();
}
```

:::info
Changes made in a [PropertyDrawer](https://docs.unity3d.com/ScriptReference/PropertyDrawer.html) will persist if the editor that draws them performs the logic.
:::  

### Sub-properties
Going levels deeper requires [`FindPropertyRelative`](https://docs.unity3d.com/ScriptReference/SerializedProperty.FindPropertyRelative.html).

```csharp
private SerializedProperty ... active, configuration;

private void OnEnable()
{
    ...
    // As Active is public we can use nameof to robustly get its name
    active = data.FindPropertyRelative(nameof(Data.Active));
    configuration = data.FindPropertyRelative(nameof(Data.Configuration));
}
```

```nomnoml
<<Nomnoml/shared.nomnoml>>

[<hidden>Data]
[Data]->[SerializedProperty (data)]
[SerializedProperty (data)|]
[<label>FindPropertyRelative("Active")]
[<label>FindPropertyRelative("Configuration")]
[SerializedProperty (data)]-[FindPropertyRelative("Active")]
[FindPropertyRelative("Active")]->[SerializedProperty (Active)]
[SerializedProperty (data)]-[FindPropertyRelative("Configuration")]
[FindPropertyRelative("Configuration")]->[SerializedProperty (Configuration)]

[SerializedProperty (Active)|]
[SerializedProperty (Configuration)|]
```

### Values

You **cannot** retrieve the C# instance associated with a SerializedProperty that isn't the bottom of the serialization hierarchy. So, in our example, we cannot retrieve the value for `data` from its SerializedProperty, we can only go deeper and get the value of the last descendants.  
Once at a SerializedProperty that is at the bottom there are predefined *Value* properties that can be used to access the value Unity has serialized.  
See the [SerializedProperty](https://docs.unity3d.com/ScriptReference/SerializedProperty.html) Properties documentation to find the appropriate Value property; such as `floatValue`, `stringValue`, or `objectReferenceValue`.

```nomnoml
<<Nomnoml/shared.nomnoml>>
#.red: stroke=#ccaa99

[<hidden>A]
[<hidden>B]
[<hidden>C]
[A]->[SerializedProperty (Active)]
[B]->[SerializedProperty (Configuration)]
[C]->[SerializedProperty (data)]

[SerializedProperty (Active)|]
[SerializedProperty (Configuration)|]
[<label>boolValue]
[<label>objectReferenceValue]
[SerializedProperty (Active)]--[boolValue]
[SerializedProperty (Configuration)]--[objectReferenceValue]
[boolValue]-->[bool (Active)]
[objectReferenceValue]-->[UnityEngine.Object (Configuration)]

[<red>SerializedProperty (data)|]
```

### Arrays
#### Iteration & access
Members in arrays are SerializedProperties themselves, you can iterate an array using the `arraySize` limit, eg:
```csharp
for (int i = 0; i < values.arraySize; i++)
{
    SerializedProperty element = values.GetArrayElementAtIndex(i);
    // element.floatValue is now accessible
}
```

```nomnoml
<<Nomnoml/shared.nomnoml>>
[<hidden>Data]
[Data]->[SerializedProperty (values)]
[SerializedProperty (values)|arraySize: int]
[<label>GetArrayElementAtIndex(i)]
[SerializedProperty (values)]-[GetArrayElementAtIndex(i)]
[GetArrayElementAtIndex(i)]->[SerializedProperty (values\[0..arraySize\])]
[SerializedProperty (values\[0..arraySize\])|]
```

#### Adding elements
Adding elements to the end of the array
```csharp
// Increase the size of the array
values.arraySize++;
// Unity has initialised lastElement to default values
SerializedProperty lastElement = values.GetArrayElementAtIndex(values.arraySize - 1);
```

Inserting elements into the array is achieved with [`InsertArrayElementAtIndex`](https://docs.unity3d.com/ScriptReference/SerializedProperty.InsertArrayElementAtIndex.html).
#### Removing elements
Use [`DeleteArrayElementAtIndex`](https://docs.unity3d.com/ScriptReference/SerializedProperty.DeleteArrayElementAtIndex.html) to remove an element at an array index.  
If the type is Object Reference you may need to set [`objectReferenceValue`](https://docs.unity3d.com/ScriptReference/SerializedProperty-objectReferenceValue.html) to null beforehand, or else a call to this method will nullify the reference and not remove the element.  



### Objects
Every new `UnityEngine.Object` type in the serialization hierarchy is a new `SerializedObject`. This means that using `FindPropertyRelative` will not iterate its children, as the children are a part of a different hierarchy.  
To iterate the children of another object you need to instance a new SerializedObject.

```csharp
private SerializedObject configurationSO;

private void OnEnable()
{
    ...
    if(configuration.objectReferenceValue != null)
        configurationSO = new SerializedObject(configuration.objectReferenceValue);
}
```

```nomnoml
<<Nomnoml/shared.nomnoml>>

[<hidden>Data]
[Data]->[SerializedProperty (Configuration)]

[SerializedProperty (Configuration)|]
[SerializedObject (Configuration)||UpdateIfRequiredOrScript()
ApplyModifiedProperties()
...]

[Configuration]
[<label>targetObject]
[Configuration]<--[<label>targetObject]
[<label>targetObject]--[SerializedObject (Configuration)]

[<label>objectReferenceValue]
[SerializedProperty (Configuration)]--[<label>objectReferenceValue]
[<label>objectReferenceValue]-->[Configuration]

[SerializedProperty (color)|colorValue: Color]
[SerializedProperty (dimensions)|vector3Value: Vector3]

[<label>FindProperty("color")]
[<label>FindProperty("dimensions")]

[SerializedObject (Configuration)]-[<label>FindProperty("color")]
[<label>FindProperty("color")]->[SerializedProperty (color)]

[SerializedObject (Configuration)]-[<label>FindProperty("dimensions")]
[<label>FindProperty("dimensions")]->[SerializedProperty (dimensions)]
```

This is a new hierarchy to find properties within, and needs to have `ApplyModifiedProperties` called **separately**.  
This object will not update if the SerializedProperty value is changed, so the reference will have to be maintained.

```csharp
public override void OnInspectorGUI()
{
    ...
    using (var changeScope = new EditorGUI.ChangeCheckScope())
    {
        EditorGUILayout.PropertyField(configuration);
        if (changeScope.changed)
        {
            // If the configuration changed, dispose of the old one, and ensure the SerializedObject is the same
            configurationSO?.Dispose();
            configurationSO = configuration.objectReferenceValue != null ?
                new SerializedObject(configuration.objectReferenceValue) :
                null;
        }
    }
    
    if (configurationSO != null)
    {
        SerializedProperty color = configurationSO.FindProperty("color");
        SerializedProperty dimensions = configurationSO.FindProperty("dimensions");
        if (GUILayout.Button("Randomise Example"))
        {
            color.colorValue = Random.ColorHSV(0, 1);
            dimensions.vector3Value = new Vector3(Random.value, Random.value, Random.value);
            configurationSO.ApplyModifiedProperties();
        }
    }
    ...
}
```

Alternatively to manually drawing a nested ScriptableObject's members you can use [CreateEditor](https://docs.unity3d.com/ScriptReference/Editor.CreateEditor.html) to create an [Editor](https://docs.unity3d.com/ScriptReference/Editor.html), and call `OnInspectorGUI` manually. [CreateCachedEditor](https://docs.unity3d.com/ScriptReference/Editor.CreateCachedEditor.html) can be used to maintain the editor when the serializedObject is changed.  
When possible SerializedObjects and Editors you create should be Disposed of when they are no longer used.  

```csharp
private void OnDisable()
{
    configurationSO?.Dispose(); // Release the native data
    configurationSO = null; // Optionally release the C# reference
}
```

### Manual property iteration
Serialized Property is actually an iterator, and in more advanced setups can be used as a part of a loop to retrieve all children of a property.  

```csharp
SerializedProperty end = property.GetEndProperty();
while (property.Next(true) && !SerializedProperty.EqualContents(property, end))
{
    // Use property
}
```

When you iterate a property this will change its internal state. If you wish to not modify the original property, [`Copy`](https://docs.unity3d.com/ScriptReference/SerializedProperty.Copy.html) it and iterate the copy instead.  