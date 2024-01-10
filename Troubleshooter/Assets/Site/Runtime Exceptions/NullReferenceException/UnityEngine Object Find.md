## NullReferenceException: UnityEngine.Object — Find
Expand the function you use and confirm you use it correctly:

#### [`GameObject.Find`](https://docs.unity3d.com/ScriptReference/GameObject.Find.html){.foldout}
Searches for a GameObject by name.
1. Only active GameObjects can be found.
1. Names are case-sensitive.
1. If your path starts with a `/` then the path must begin at the root of the scene.
1. Assignment must occur before access. Often you would use [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) to get, and [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html) to use.

#### [`Transform.Find`](https://docs.unity3d.com/ScriptReference/Transform.Find.html){.foldout}
Searches for children by name.
1. Only direct children are returned unless a path is provided.
1. Names are case-sensitive.
1. If a path is provided, the path should look like: `Child/Descendant/Etc`.
1. [Search the scene](../../Scene%20View/Searching.md) for all instances of the caller and check all results (`t:ExampleComponent` for example).
1. Assignment must occur before access. Often you would use [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) to get, and [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html) to use.

#### [`GameObject.FindWithTag`](https://docs.unity3d.com/ScriptReference/GameObject.FindWithTag.html){.foldout}
1. Check the console for a `UnityException`. `FindWithTag` will throw an exception when used with a tag that does not exist.
1. Only active GameObjects can be found.
1. Assignment must occur before access. Often you would use [`Awake`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) to get, and [`Start`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html) to use.

---

:::warning
Double-check the assignment is actually executed. Your code may never called because of an incorrect setup like a misspelt method name.
:::

Find is an expensive method, see [alternatives](../../References.md).  
