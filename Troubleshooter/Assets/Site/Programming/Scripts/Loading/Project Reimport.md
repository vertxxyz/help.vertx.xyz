## Project reimport
### Description
The Library folder contains the imported data that makes up your project.  
All of this data is generated from the assets and code, and can be removed and regenerated.

### Resolution
#### Script Assemblies
Deleting the script assemblies folder will reset most code-related issues, and only requires a partial project reimport.  
1. Close Unity.
2. Delete the **Library/ScriptAssemblies** folder at the root of the project.
3. Reopen Unity.

#### Entire Library
Deleting the library folder will reset all imported code and assets, requiring a full project reimport. This can take a long time for projects with many assets.  
1. Close Unity.
2. Delete the **Library** folder at the root of the project.
3. Reopen Unity.

---  
Sorry, we've run out of troubleshooting!  
If you resolved your issue and the fix was not listed in the [troubleshooting steps](Script%20Name.md), please <<report-issue.html>>.