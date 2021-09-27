*[asmdefs]: Assembly Definitions
*[asmdef]: Assembly Definition
## Assembly Definitions
:::warning
Editor folders **do not** function under assembly definitions
:::

It is recommended to have a Runtime and Editor asmdefs separated into different folders at a root level, unlike the usual paradigm of having editor folders scattered throughout your project.  

An editor asmdef should only include **Editor** under the Platforms selection.  
Ensure that **Any Platform** is switched **off**, *Deselect All*, and then only tick **Editor**.

![Editor Assembly Definition](editor-asmdef.png)