<<Abbreviations/TMP.md>>  
## Parsing input strings
### Issue
When attempting to parse strings from input fields they may have invisible characters appended. These characters must be removed to parse the string into a result like an `int`.  
Hidden characters will also cause issues when attempting to compare to these strings.  

### Resolution
When referencing text from a TMP input field, do not reference the underlying [`TextMeshProUGUI`](https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest/index.html?subfolder=/api/TMPro.TextMeshProUGUI.html), reference the [`TMP_InputField`](https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest/index.html?subfolder=/api/TMPro.TMP_InputField.html) itself.  
:::warning{.inline}
The child `TextMeshProUGUI` uses a zero-width space for layout purposes, and should not be referenced.
:::

```csharp
[SerializeField]
private TMP_InputField _inputField;

public void UseInput()
{
    string text = _inputField.text;
    // Use text
}
```

### Notes
#### Use TryParse!
It's almost always worthwhile to use the `TryParse` variants of parsing functions to ensure proper handling of a parsing failure.  
<<Code/Parsing/Parsing.rtf>>  

#### Trimming whitespace
To remove whitespace from strings you can generally use the [`.Trim()`](https://docs.microsoft.com/en-us/dotnet/api/system.string.trim?view=net-6.0) function, which **returns** a modified string. More complex removal may require the use of [Regex](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=net-6.0), a complex and powerful language used to build patterns for searching text. Search for regex tutorials to get started on that journey.  

The `Trim()` function *may* suffice when removing invisible characters from a user, but the child TMP object has an appended character that would need to be removed more manually:  
<<Code/Parsing/Input Trim.rtf>>  