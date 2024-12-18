---
title: "Script loading issues"
description: "\"Can't add script!\" - Troubleshooting broken scripts."
image: "script-loading-issues.png"
---
# Script loading issues
```
Can't add script component 'Foo' because the script class cannot be found.
Make sure that there are no compiler errors and the file name and class name match.
```

```
'Foo' is missing the class attribute 'ExtensionOfNativeClass'!
```

```
The script don't inherit a native class that can manage a script.  
```

## Troubleshooting steps
Make sure your script is saved, then follow these steps:

1. [Check class and file names.](Script%20Loading%20Issues/Script%20Name.md)
2. [Resolve Console errors.](Script%20Loading%20Issues/Console%20Errors.md)
```
Can't add script behaviour 'Foo'. The script needs to derive from MonoBehaviour!
```
3. [Check for correct Unity inheritance.](Script%20Loading%20Issues/Base%20Type.md)

```
The script is an editor script.
```

4. [Ensure your script isn't in an Editor context.](Script%20Loading%20Issues/Editor%20Contexts.md)
    1. [Check for Editor folders.](Script%20Loading%20Issues/Editor%20Folders.md)
    1. [Check for Editor Assembly Definitions.](Script%20Loading%20Issues/Assembly%20Definitions.md)
5. [Restart Unity.](Script%20Loading%20Issues/Restart%20Unity.md)
6. [Reimport the project.](Script%20Loading%20Issues/Project%20Reimport.md)
