<<Abbreviations/Asmdef.md>>
## Assembly Definitions
:::warning
Editor folders don't function under Assembly Definitions
:::

It is recommended to have a Runtime and Editor asmdefs separated into different folders at a root level, unlike the usual paradigm of having editor folders scattered throughout your project.  

An editor asmdef should only include **Editor** under the Platforms selection.  
Check that **Any Platform** is switched **off**, *Deselect All*, and then tick **Editor** only.

![Editor Assembly Definition](editor-asmdef.png)