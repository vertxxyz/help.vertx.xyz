## NullReferenceException: UnityEngine.Object — Singletons

:::note
#### 1. Assign your singleton in `Awake`
Singletons should be initialized before other code runs, so assign a value to `Instance` in [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html).  
See [singletons](../../References/Singletons.md) to learn more about setup.

:::

:::note
#### 2. Do not read from a Singleton in `Awake` or `OnEnable`
Using a singleton in `Awake` or `OnEnable` may result in reading from it before it has been initialized.  

You should always attempt to configure an object in `Awake`, and read from its dependencies in a later method like [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html).

:::

:::note
#### 3. Ensure the singleton exists in the scene
1. Make sure the singleton has been added as a component on an object in the scene.
1. Make sure nothing destroys the singleton, or has destroyed it completely during a scene change.

If the singleton should persist, you may choose to use [`DontDestroyOnLoad`](https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html) to keep it around when the scene is unloaded.
:::
