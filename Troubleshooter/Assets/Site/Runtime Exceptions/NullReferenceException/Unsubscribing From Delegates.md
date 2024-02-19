## NullReferenceException: Delegates
Subscribing to a delegate without unsubscribing can be a memory leak.  

Under certain conditions—especially if [Configurable Enter Play Mode](https://docs.unity3d.com/Documentation/Manual/ConfigurableEnterPlayMode.html) is enabled—`static` delegates will persist between Play Mode sessions. If subscribers are not cleaned up then this can also be a leak.

These leaks can cause a `NullReferenceException` when they reference a Unity Object that was destroyed (destroyed manually, or destroyed by exiting Play Mode).

### Resolution
Unsubscribe from delegates—especially those that are `static`— when your object is destroyed or you should no longer be subscribed.

```csharp
// Unsubscribing a single subscriber 
s_MyStaticEvent -= Subscriber;

// Removing all subscribers
s_MyStaticEvent = null;
```

You can generally do this from [`OnDestroy`](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDestroy.html), which will be called when a Component is destroyed (destroyed manually, or destroyed by exiting Play Mode).

#### Configurable Enter Play Mode
If you are using Configurable Enter Play Mode, you should unsubscribe explicitly using the callback [mentioned in the documentation](https://docs.unity3d.com/Manual/DomainReloading.html).
```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
static void Unsubscribe()
{
    s_MyStaticEvent = null;
}
```

---

[Return to NullReferenceException: UnityEngine.Object.](UnityEngine%20Object%20Assignment.md)
