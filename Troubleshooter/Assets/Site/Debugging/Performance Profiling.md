## Performance profiling
To profile the overall performance of your application, use a profiling tool.  
:::info
Prefer profiling builds to get accurate performance information regarding your application.
:::

### Unity Profiler
The built-in Unity Profiler can be opened via **Window | Analysis | Profiler**. It provides a per-frame breakdown of performance in the form of a hierarchy or timeline.  
[Learn more.](https://docs.unity3d.com/Manual/Profiler.html)

To compare multiple profiling captures you can use the Profile Analyzer (`com.unity.performance.profile-analyzer`).  
[Learn more.](https://docs.unity3d.com/Packages/com.unity.performance.profile-analyzer@latest)

You can integrate your own profiling information directly into the Unity Profiler using the Profiling Core package (`com.unity.profiling.core`).  
[Learn more.](https://docs.unity3d.com/Packages/com.unity.profiling.core@latest)

### Superluminal
[Superluminal](https://superluminal.eu/unity/) is a sampling profiler that can profile both managed and native code. It's a great alternative that can provide deeper insight into performance issues, especially those occurring over many frames.  
[Learn more.](Superluminal.md)
