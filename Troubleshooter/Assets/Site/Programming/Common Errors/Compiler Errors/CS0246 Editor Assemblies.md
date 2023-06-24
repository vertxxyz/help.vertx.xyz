## [CS0246](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0246): Editor assemblies

```
The type or namespace name 'Foo' could not be found (are you missing a using directive or an assembly reference?)
```

### Description
Unity has a number of [special folders](https://docs.unity3d.com/Manual/ScriptCompileOrderFolders.html) that are reserved for various purposes. A few of these control which assemblies scripts end up in.

| Phase | Assembly name                    | Script files                                                                                                                                |
|-------|----------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------|
| 1     | Assembly-CSharp-firstpass        | Runtime scripts in folders called Standard Assets, Pro Standard Assets and Plugins.                                                         |
| 2     | Assembly-CSharp-Editor-firstpass | Editor scripts in folders called Editor that are anywhere inside top-level folders called Standard Assets, Pro Standard Assets and Plugins. |
| 3     | Assembly-CSharp                  | All other scripts that are not inside a folder called Editor.                                                                               |
| 4     | Assembly-CSharp-Editor           | All remaining scripts (those that are inside a folder called Editor).                                                                       |

:::warning{.inline}  
Anything that is compiled in a phase after the current one cannot be referenced.
:::  

You can bypass these rules and set up your on assemblies using [Assembly Definitions](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html).

### Resolution

Make sure you're not breaking the reference rules listed in the above description. For example, code outside of an Editor folder cannot reference code inside one, because the editor assembly is compiled after the runtime one.  

#### Common mistakes
- `PropertyAttribute` and `PropertyDrawer`
  - The `PropertyAttribute` should be in the runtime assembly so it can be referenced by runtime code.
  - The `PropertyDrawer` remains in the Editor assembly.
- Accidentally naming a folder "Editor" when it should contain runtime code.

---

[I am still having issues referencing a type.](CS0246%20Other%20Considerations.md)