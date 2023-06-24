## [CS0246](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0246): Assembly Definitions

```
The type or namespace name 'Foo' could not be found (are you missing a using directive or an assembly reference?)
```

### Description
Assembly Definitions (asmdefs) are used to manually group code into individual assemblies.  
They also define the references between those assemblies, giving you tighter control over your compilation.  

[Learn more about Assembly Definitions.](https://learn.unity.com/tutorial/working-with-assembly-definitions)  

### Resolution
Navigate to the heading that fits your setup and complete the listed steps.

:::note  
#### Both scripts are under separate Assembly Definitions
1. Select the asmdef that contains the current script.
1. In the **Assembly Definition References** section of the inspector add a reference to the asmdef that contains the script you wish to reference.
1. Make sure you're not trying to reference an Editor-only assembly (see image) from a runtime one.
1. Apply and wait for Unity to recompile.  
   The reference should now be resolved when the namespace is added.

^^^
![Editor Assembly Definition](../../../Building/editor-asmdef.png)
^^^ Editor-only Assembly Definition
:::  
:::note
#### This script is under an Assembly Definition, the target is in a plugin/DLL
1. Select the asmdef that contains the current script.
1. Under **General**, tick **Override References**.
1. In the **Assembly References** section of the inspector add a reference to the target plugin.
1. Apply and wait for Unity to recompile.  
   The reference should now be resolved when the namespace is added.  

:::  
:::note
#### This script is under an Assembly Definition, the target isn't
If the target is Unity's assemblies **No Engine References** must not be ticked on the asmdef.  

If the target is in your project but not in an asmdef it cannot be referenced by code in an Assembly Definition.  
You may want to consider adding asmdefs to all parts of your project where possible. Adding them to external plugins for compatibility with your project is a normal thing to do.  
:::  
:::note
#### This script isn't under an Assembly Definition, the target is
Scripts outside of asmdefs can reference asmdef assemblies if **Auto Referenced** is ticked on their asset.  
If it isn't and can't be changed, then this code must be moved under its own asmdef to have it reference that assembly.  

:::  
:::note
#### Neither scripts are under Assembly Definitions
You can search the project with `t:AssemblyDefinitionAsset` to find asmdefs in the project, making sure none are above your scripts.

Continue debugging with [Editor assemblies.](CS0246%20Editor%20Assemblies.md)  
:::

---

[I am still having issues referencing a type.](CS0246%20Other%20Considerations.md)