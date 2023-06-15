## Valid references diagram

<script type="module" src="/Scripts/Interactive/References/validReferences.js?v=1.0.1"></script>  

This directional diagram describes valid serialized references.

:::{.nomnoml-group}

```nomnoml
<<Nomnoml/shared.nomnoml>>
#title: ref_overview
[Scene ᵃ]->[Project assets]
[Scene ᵃ]->[Package assets]
[Scene ᵇ]->[Project assets]
[Project assets]->[Package assets]
[Scene ᵇ]->[Package assets]
[<reference>Assets]
[Package assets]--[Assets]
[Project assets]--[Assets]
```

```nomnoml
<<Nomnoml/shared.nomnoml>>
#title: ref_assets
#gutter: 6
[<reference> Assets]
[Assets]-[<hidden>a]->[Prefab asset|
    [<hidden>]<->[Children]
]
[<hidden>]->[Asset or sub-asset]
[<hidden>]->[Prefab asset]
[Assets]-[<hidden>b]->[Asset or sub-asset]
```
:::

:::note{#ref-tooltip .table-children--expand }  
::::{.center}  
Hover or select elements for more information.  
::::  
:::

:::note  
#### Key
```nomnoml
<<Nomnoml/shared.nomnoml>>
#direction:right
#leading: 1
#fontSize: 10
#gutter: 1
#spacing: 20
#.assets: dashed visual=frame title=left fill=#28282878
[<label>Content location]-/-[Target A]
[<label>One way reference]->[Target B]
[<label>Two way reference]<->[Target C]
[<label>Reference to another diagram]--[<reference>Diagram]--[<hidden>]
```

:::

:::::{.hidden #ref-tooltip-data}  

::::{ data-title="ref_overview" data-keys="Scene ᵃ, Scene ᵇ" }
+-------------------+--------------+------------+  
|                   | From a scene | To a scene |  
+===================+==============+============+  
| Other **scenes**  |N             |N           |  
+-------------------+--------------+------------+  
| **Project**       |Y             |N           |  
| assets            |              |            |  
+-------------------+--------------+------------+  
| **Packaged**      |Y             |N           |  
| assets            |              |            |  
+-------------------+--------------+------------+  
:::warning{.inline}  
See Assets diagram for asset reference specifics  
:::  
::::  

::::{ data-title="ref_overview" data-keys="Project assets" }  
+-----------------------+---------------------+-------------------+  
|                       | From project assets | To project assets |  
+=======================+=====================+===================+  
| **Scenes**            |N                    |Y                  |  
+-----------------------+---------------------+-------------------+  
| Other **project**     |Y                    |Y                  |  
| assets                |                     |                   |  
+-----------------------+---------------------+-------------------+  
| **Packaged**          |Y                    |N                  |  
| assets                |                     |                   |  
+-----------------------+---------------------+-------------------+  
:::warning{.inline}  
See Assets diagram for asset reference specifics  
:::  
::::  

::::{ data-title="ref_overview" data-keys="Package assets" }  
+-----------------------+---------------------+-------------------+  
|                       | From package assets | To package assets |  
+=======================+=====================+===================+  
| **Scenes**            |N                    |Y                  |  
+-----------------------+---------------------+-------------------+  
| **Project**           |N                    |Y                  |  
| assets                |                     |                   |  
+-----------------------+---------------------+-------------------+  
| Other **packaged**    |Y                    |Y                  |  
| assets                |                     |                   |  
+-----------------------+---------------------+-------------------+  
:::warning{.inline}  
See Assets diagram for asset reference specifics  
:::  
::::  

::::{ data-title="ref_assets" data-keys="Asset or sub-asset" }
+-----------------------+---------------------------+-------------------------+  
|                       | From assets or sub-assets | To assets or sub-assets |  
+=======================+===========================+=========================+  
| Scenes                |N                          |Y                        |  
+-----------------------+---------------------------+-------------------------+  
| Other                 |Y                          |Y                        |  
| assets or sub-assets  |                           |                         |  
+-----------------------+---------------------------+-------------------------+  
| Prefab assets         |Y                          |Y                        |  
+-----------------------+---------------------------+-------------------------+  
| Prefab asset children |N                          |Y                        |  
+-----------------------+---------------------------+-------------------------+  
:::warning{.inline}  
See Scene/Project/Package diagram for location specifics  
:::  
::::  

::::{ data-title="ref_assets" data-keys="Prefab asset" }
+-----------------------+---------------------------+-------------------------+  
|                       | From prefab assets        | To prefab assets        |  
+=======================+===========================+=========================+  
| Scenes                |N                          |Y                        |  
+-----------------------+---------------------------+-------------------------+  
| Assets or sub-assets  |Y                          |Y                        |  
+-----------------------+---------------------------+-------------------------+  
| Other                 |Y                          |Y                        |  
| prefab assets         |                           |                         |  
+-----------------------+---------------------------+-------------------------+  
| This prefab's         |Y                          |Y                        |
| children              |                           |                         |  
+-----------------------+---------------------------+-------------------------+  
| Other prefab's        |N                          |Y                        |
| children              |                           |                         |  
+-----------------------+---------------------------+-------------------------+  
:::warning{.inline}  
See Scene/Project/Package diagram for location specifics  
:::  
::::  

::::{ data-title="ref_assets" data-keys="Children" }
+-----------------------+----------------------------+--------------------------+  
|                       | From prefab asset children | To prefab asset children |  
+=======================+============================+==========================+  
| Scenes                |N                           |N                         |  
+-----------------------+----------------------------+--------------------------+  
| Assets or sub-assets  |Y                           |N                         |  
+-----------------------+----------------------------+--------------------------+  
| Other                 |Y                           |N                         |  
| prefab assets         |                            |                          |  
+-----------------------+----------------------------+--------------------------+  
| This prefab's         |Y                           |Y                         |
| children              |                            |                          |  
+-----------------------+----------------------------+--------------------------+  
| Other prefab's        |N                           |N                         |
| children              |                            |                          |  
+-----------------------+----------------------------+--------------------------+  
:::warning{.inline}  
See Scene/Project/Package diagram for location specifics  
:::  
::::

:::::

### Description
#### Scene
Any content in the hierarchy of a [scene](https://docs.unity3d.com/Manual/CreatingScenes.html).
#### Project assets
Any content in the [project](https://docs.unity3d.com/Manual/ProjectView.html). This is any asset in the Assets or Project Settings directories.
#### Package assets
Any content in a [package](https://docs.unity3d.com/Manual/PackagesList.html). These are assets that appear in the Packages directory in the [project view](https://docs.unity3d.com/Manual/ProjectView.html). Assets from Unity's Asset Store may use the Package Manager but are not installed as packages.
#### Prefab assets
GameObjects in the project packages. Prefab instances in scenes refer to prefab assets, but are not assets themselves.
#### Prefab asset children
GameObjects under the root of prefab assets.
#### Assets and sub-assets
Project or package assets that are not prefabs.

### How-to
If you're unsure how to reference one location from another, refer to the [how-to](Serialized%20References.md#how-to) section.