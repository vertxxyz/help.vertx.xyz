<<Abbreviations/TMP.md>>  
## Parsing input strings
### Issue
When working with strings, they may have invisible characters appended.  
When referencing an input field, take care to not reference text field directly.  
In other cases, these hidden characters must be removed. They will also cause issues when attempting to compare strings.

### Resolution
#### Text mesh pro input fields
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

#### Use TryParse!
It's almost always worthwhile to use the `TryParse` variants of parsing functions to ensure proper handling of a parsing failure.  
<<Code/Parsing/Parsing.rtf>>  

#### Trimming whitespace
To remove whitespace from strings you can generally use the [`.Trim()`](https://docs.microsoft.com/en-us/dotnet/api/system.string.trim?view=net-6.0) function, which **returns** a modified string. More complex removal may require the use of [Regex](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=net-6.0), a complex and powerful language used to build patterns for searching text. Search for regex tutorials to get started on that journey.  

The `Trim()` function *may* suffice when removing invisible characters from a user, but the child TMP object has an appended character that would need to be removed more manually:  
<<Code/Parsing/Input Trim.rtf>>  

#### Debugging
1. Check the Length of your string and compare it to what you expect.
1. Index into the string to find the problematic character, and convert the result to hex. String is UTF-16, look up what the corresponding character is.

```csharp
Debug.Log($"The problematic character is U+{(int)text[index]:X4}");
```

#### Common hidden characters

| Character as code | UTF-16 | Description                 |
|-------------------|--------|-----------------------------|
| ` `               | U+0020 | Space (SP)                  |
| `\n`              | U+000A | Line feed (LF)              |
| `\r`              | U+000D | Carriage return (CR)        |
| `\t`              | U+0009 | Character tabulation (TAB)  |
| `\u200b`          | U+200B | Zero-width space[^1] (ZWSP) |


`\r\n`, or CRLF is a common combination to denote a new line on Windows. Mac and Unix both use `\n`, LF.

[^1]: See the first resolution on this page if you are encountering a zero width space.