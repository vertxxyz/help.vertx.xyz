---
title: "Referring to members in other scripts"
description: "Choose the best way to reference other variables."
---
# Referring to members in other scripts

There are various ways to create a reference between one object and another:

## Serialized references
Serialized references are exposed via the inspector and are defined per-instance. They are fast and configurable.  
[Learn more.](References/Serialized%20References.md)

## Singletons
Singletons are referenced in code and require **only one instance** of the target type.  
[Learn more.](References/Singletons.md)

## GetComponent methods
Prefer serialized references for their speed and configurability. [`GetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html), [`TryGetComponent`](https://docs.unity3d.com/ScriptReference/GameObject.TryGetComponent.html),
[`GetComponentInChildren`](https://docs.unity3d.com/ScriptReference/Component.GetComponentInChildren.html), and other similar methods are perfect for dynamic runtime references, like those resolved during collisions and scene queries.  
[Learn more.](References/GetComponent%20Methods.md)

## Find methods
Avoid the various find methods unless you are debugging or prototyping. These methods are often extremely slow, and even when used from `Awake` or `Start` can cause hitches during loading.
Learn more about `Find` methods in the static methods list on [`GameObject`](https://docs.unity3d.com/ScriptReference/GameObject.html).

Prefer [serialized references](References/Serialized%20References.md), [singletons](References/Singletons.md), or [GetComponent](References/GetComponent%20Methods.md).
If you are referencing objects you spawned at runtime, add them to a collection (like a [List](https://learn.unity.com/tutorial/lists-and-dictionaries)) when they are created, and reference that object to get your instances instead.

## Dependency injection
Dependency injection (DI) is simply the process of having a reference passed to an object. [Serialized references](References/Serialized%20References.md) are already a simple form of DI, but you can apply the concept yourself and inject references from managers to other objects under their control.  
[Learn more.](References/Simple%20Dependency%20Injection.md)

## Alternate methods
There are other ways to refer to external members, including varied use of the `static` keyword. Generally these can be avoided until users are familiar with more basic and common concepts.
