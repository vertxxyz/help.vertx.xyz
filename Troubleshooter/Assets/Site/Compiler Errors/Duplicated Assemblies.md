---
title: "Duplicated assemblies"
description: "Assembly with name 'X' already exists (.../X.asmdef)"
---
# Duplicated assemblies
```
Assembly with name 'X' already exists (.../X.asmdef)
```

There should only be one assembly in your project with a certain name.

## Resolution
Do not name your own assemblies the same as one already existing in your project.  
If the assembly is not yours, make sure there isn't a duplicate. Sometimes people have packages placed in their Assets directory that are already present in Packages. If this is the case, delete the package from Assets.  

You can see packages in your project via the Package Manager (**Window | Package Manager**), and also via the Packages folder visible in the Project window.
