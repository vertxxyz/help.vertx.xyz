<<Abbreviations/TMP.md>>  
### Parsing Input Strings
#### Issue
When attempting to parse strings from Input Fields the input field may have appended invisible characters. These characters must be removed to parse the string into a result like an `int`.

#### Resolution
The `Trim()` function will often suffice when removing invisible characters, but I have often found TMP has appended characters that need to be removed more manually.  
One of the characters is a specific whitespace character that can be removed like:  
<<Code/Parsing/Input Trim.rtf>>  

#### Notes
It's almost always worthwhile to use the `TryParse` variants of parsing functions to ensure proper handling of a parsing failure.  
<<Code/Parsing/Parsing.rtf>>  