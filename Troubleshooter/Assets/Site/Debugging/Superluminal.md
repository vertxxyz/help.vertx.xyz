# Superluminal
[Superluminal](https://superluminal.eu/unity/) is a sampling profiler, it halts the CPU at 8-40khz (depending on platform) and reads the state of the stack. So more data is better data, and looking at individual traces might not produce *extremely* accurate results like a traditional instrumentation profiler. So when you're looking at timings note that while you can use it like Unity's profiler, you'll likely get better results by selecting a large range of the capture that you think contains the issue.

## Setup
1. Install Superluminal and note where you installed it.
1. In the Unity Hub use the dropdown next to the project to **Add Command line arguments**. Add `-monoProfiler superluminal`
1. Copy the mono profiling dll from Superluminal to Unity:
   1. In the Hub, find your 2019.4.40 install and show in explorer.
   1. Find the Superluminal install and open the **Unity > x64** directory.
   1. Copy the `mono-profiler-superluminal.dll` into the Unity install directory, next to the exe.
1. Reopen the Unity project.
1. In Superluminal add `http://symbolserver.unity3d.com/` to the symbol file locations.
1. When you use the **Attach** menu to attach to Unity.exe, it will start profiling immediately, so only do it when you're ready to profile!
