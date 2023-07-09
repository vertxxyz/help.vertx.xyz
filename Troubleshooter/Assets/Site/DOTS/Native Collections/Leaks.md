## Native collections: Leaks
### Finding leaks
Switch **Edit | Preferences | Jobs | Leak Detection Level** to enabled with stack trace. Leaks created once this setting is enabled should retain a stack trace from where the allocation occured, which should be appended to the log.

:::warning{.inline}
This setting will reset when you restart Unity.
:::

Make sure the [Collections](https://docs.unity3d.com/Packages/com.unity.collections@latest) package is up to date, version 2.1.1 fixed collection tracking. You may need to update it manually via the [project manifest](https://docs.unity3d.com/Manual/upm-manifestPrj.html).

If you do not get stack traces after enabling this setting, it could be a version issue, leak detection is under development to support complex Entities use-cases (bursted `ISystem` for example).

### Resolving leaks
Non-Temp-allocated native collections must be manually disposed using the `Dispose` function.  
If the collection has job dependencies, pass the `JobHandle` created by the job into the `Dispose` function to release the collection after the job runs.