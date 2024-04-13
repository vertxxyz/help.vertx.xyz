# MissingReferenceException
A MissingReferenceException is a type of [NullReferenceException](NullReferenceException.md) where a Unity Object field was previously assigned to, and the contents have been deleted or become invalid.

It can also occur when using `GetComponent` in cases where the component was not present.

## Resolution
First, read the error message and understand what types and variables it's talking about.  
:::info{.small}  
In some cases selecting the error message in the Unity Console will ping the object that threw it.  
:::  

### Then choose the type of MissingReferenceException that relates to your message:

::::note
### ::MissingReferenceException: The variable _Foo_ of _Bar_ doesn't exist anymore.::{.code-block--no-background}

Re-assign a value to the field mentioned (*Foo*) to all instances of the script (*Bar*) via the Inspector.

<video width="750" height="200" autoplay loop muted controls><source type="video/webm" src="https://unity.huh.how/Video/inspector-references.webm"></video>
::::

::::note  
### ::MissingReferenceException: There is no '_Foo_' attached to the "_Bar_" game object.::{.code-block--no-background}

Check that the mentioned component (*Foo*) is attached to all game object instances named (*Bar*) in the scene.

Alternatively, if you're using `GetComponent` and want to check if the component was correctly found, use `TryGetComponent` instead:

```csharp
if (TryGetComponent(out Foo foo))
{
    // Foo has been found correctly.
}
```

---

:::warning{.small}
This can also occur when trying to retrieve something that isn't a `Component` using the [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html) functions.  
:::  
For example, `There is no 'GameObject' attached to the "Example" game object`, is caused by `GetComponent<GameObject>()`.
To get a GameObject from a component Unity exposes the [gameObject](https://docs.unity3d.com/ScriptReference/Component-gameObject.html) property.  
::::

::::note
### ::MissingReferenceException: The object of type '_Foo_' has been destroyed but you are still trying to access it.::{.code-block--no-background}

You destroyed an object (an instance of *Foo*), and attempted to read from a variable that referenced it.  

Read the [stack trace](../Stack%20Traces.md), and find what call threw the exception, and figure out whether to correctly reference an existing object, or to check if the object is destroyed.

To check if an Object was destroyed in Unity, check if it's `null`:
```csharp
if (foo != null)
{
    // foo is assigned and has not been destroyed.
}
```
::::

---

To find more troubleshooting steps, read through the [NullReferenceException](NullReferenceException.md) guide.
