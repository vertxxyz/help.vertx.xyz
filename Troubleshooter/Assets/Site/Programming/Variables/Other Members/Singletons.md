### Singletons
#### Basics
If you control the target type and it's completely decided that there will only be one of them, then one method of easily accessing its members is to use a singleton.  
You must be wary when using `static` as it can be difficult to refactor if multiple instances are required in the future.  
<<Code/Variables/Singleton 1.rtf>>

If we defined a static variable and assigned it when the class is initialised then we can refer to that single instance via the static property:  
<<Code/Variables/Singleton 2.rtf>>

#### Details
This concept can be applied for non-MonoBehaviour classes by performing similar logic in the constructor. Destroy is not needed in that case.

The above example is a simple implementation of a Singleton, but it isn't necessarily ideal. Often singletons should be set up once for the lifetime of a project, and should survive scene-loading.  
Unity has a method for doing this, `DontDestroyOnLoad` ([docs](https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html)).  
We can go one step further and make our entire class generic, enabling us to easily create singletons in the future!  
<<Code/Variables/Singleton 3.rtf>>

Usage:  
<<Code/Variables/Singleton 4.rtf>>

#### Further considerations
For more advanced users you may want to be using [Configurable Enter Play Mode](https://docs.unity3d.com/2019.3/Documentation/Manual/ConfigurableEnterPlayMode.html), which requires code to manually reset statics across different meta states in the editor.  
To implement singletons that properly work with this, more advanced inheritance hierarchies that use [RuntimeInitializeOnLoadMethod](https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html) with [SubsystemRegistration](https://docs.unity3d.com/ScriptReference/RuntimeInitializeLoadType.SubsystemRegistration.html) to reset the static members of the singleton are required.