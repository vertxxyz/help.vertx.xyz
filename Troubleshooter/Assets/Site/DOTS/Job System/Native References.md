# Getting data out of jobs

Data can only be retrieved from jobs via [native collections](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/manual/index.html).

This means you need to:
1. Allocate a native collection.
1. Pass it to a member in the job struct.
1. Run the job and wait for it to complete.
1. Read the results present in the collection.
1. Dispose of the collection when you're finished with it.

## Choosing a native collection
See [collection types](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/manual/collection-types.html) for a complete overview of native collections.

### Retrieving multiple values

| Type                                                                                                                                                                                                                                                                                                                                                                      | Description                                                     | Requires capacity | Supports parallel writes |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------|-------------------|--------------------------|
| [`NativeArray<T>`](https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeArray_1.html)                                                                                                                                                                                                                                                                         | A fixed-length data structure similar to a C# array (`T[]`).    | No                | Yes                      |
| [`NativeList<T>`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeList-1.html)                                                                                                                                                                                                                           | A data structure similar to a `List<T>`.                        | No                | Partial (no resizing)    |
| [`NativeQueue<T>`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeQueue-1.html)                                                                                                                                                                                                                         | A data structure similar to a `Queue<T>`.                       | No                | Yes                      |
| [`NativeHashSet<T>`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeHashSet-1.html)<br/>[`NativeParallelHashSet<T>`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeParallelHashSet-1.html)                                           | A data structure similar to a `HashSet<T>`.                     | Yes               | Yes                      |
| [`NativeHashMap<TKey, TValue>`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeHashMap-2.html)<br/>[`NativeParallelHashMap<TKey, TValue>`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeParallelHashMap-2.html)                     | A data structure similar to a `Dictionary<TKey, TValue>`.       | Yes               | Yes                      |
| [`NativeMultiHashMap<TKey, TValue>`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeMultiHashMap-2.html)<br/>[`NativeParallelMultiHashMap<TKey, TValue>`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeParallelMultiHashMap-2.html) | A data structure similar to a `Dictionary<TKey, List<TValue>>`. | Yes               | Yes                      |

For situations requiring parallel writing either the capacity must be specified beforehand, or the order will not be maintained. If order is required with complex structures the best approach can differ between choosing to allocate extra capacity, using double-buffering techniques to grow buffers over multiple iterations, or running multiple jobs to collect and post-process data.

Many custom structures exist that act as native bridges. You can search sources like GitHub for custom native collections such as native trees.

### Retrieving one value
The [`NativeReference`](https://docs.unity3d.com/Packages/com.unity.collections@latest/index.html?subfolder=/api/Unity.Collections.NativeReference-1.html) is the same as a `NativeArray` of length `1`, but it semantically implies a single value.

## Example

```csharp

public void RunJob()
{
    NativeArray<float> output = new(10, Allocator.TempJob);
    var job = new MyJob
    {
        Output = output,
        Multiplier = 100
    };
    
    JobHandle handle = jobData.Schedule();
    
    handle.Complete();
    
    /* Collapsable: Read output */
    // --- Read output ---
    foreach(float value in output)
    {
        Debug.Log(value);
    }
    /* End Collapsable */
    
    output.Dispose();
}

public struct MyJob : IJob
{
    public NativeArray<float> Output;
    public float Multiplier;
    
    public void Execute()
    {
        /* Collapsable: Example usage */
        // --- Example usage ---
        int length = Output.Length;
        for (int i = 0; i < length; i#)
        {
            Output[i] = i * Multiplier;
        }
        /* End Collapsable */
    }
}

```

Note that immediately completing a job is not ideal, see the [create and run a job](https://docs.unity3d.com/Manual/JobSystemCreatingJobs.html) example in the Unity documentation.
