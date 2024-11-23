---
title: "Singletons"
description: "Overview of singletons in Unity"
---
# Singletons
## Description
A singleton is a convenient access pattern for one-off data structures.  
It provides `static` access to a single instance, allowing for instance data to be serialized in the inspector, while letting the class be accessed from anywhere.


If you control the target type and it's completely decided that there will only instance, then one method to easily access its members is to use a singleton.  

:::warning
You must be wary when using `static`, it can be difficult to refactor if multiple instances are required in the future.  
:::

## Implementation

Define a `static` property of the enclosing type and assign it during initialisation:  
<<Code/Variables/Singleton 1.html>>

That `Instance` property can then be referred to in other classes via the type name:  
<<Code/Variables/Singleton 2.rtf>>

## Notes
### Singleton lifetime
The above example is a simple implementation of a Singleton, but it isn't necessarily ideal. Often singletons should be set up once for the lifetime of a project, and should survive scene-loading.  
Unity has a method for doing this called [`DontDestroyOnLoad`](https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html).  

### Generic implementation
We can go one step further and make our entire class generic, enabling us to easily create singletons in the future!  
<<Code/Variables/Singleton 3.rtf>>

Usage:  
<<Code/Variables/Singleton 4.rtf>>

### Singletons don't need to be MonoBehaviours!
This concept can be applied for non-MonoBehaviour classes by performing similar logic in the constructor. Destroy isn't needed in that case.

### Supporting Configurable Enter Play Mode
For more advanced users you may want to be using [Configurable Enter Play Mode](https://docs.unity3d.com/Documentation/Manual/ConfigurableEnterPlayMode.html), which requires code to manually reset statics across different meta states in the editor.  
To implement singletons that properly work with this, more advanced inheritance hierarchies that use [`RuntimeInitializeOnLoadMethod`](https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html) with [`SubsystemRegistration`](https://docs.unity3d.com/ScriptReference/RuntimeInitializeLoadType.SubsystemRegistration.html) to reset the static members of the singleton may be required.
