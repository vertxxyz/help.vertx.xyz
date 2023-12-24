## uGUI: Canvas size
uGUI scales your canvas to be the size of your screen or Scene view, but expressed in world units.

It is normal for a canvas to appear very large in comparison to other objects.  

### Notes
#### Focusing different objects
You can press <kbd>F</kbd> with an object selected to quickly refocus the camera between large and small elements.

#### Hiding the canvas or making it unselectable
Sometimes you want to get the canvas out of the way so you can work on other objects in the scene. 
See [Scene view visibility](../Interface/Scene%20View/Visibility.md) and [Scene view selection](../Interface/Scene%20View/Selection.md) to learn how to toggle visiblity/selection for Scene view objects.

#### Making the canvas a prefab
If you [create a prefab](https://docs.unity3d.com/Manual/CreatingPrefabs.html) out of a Canvas the prefab environment can be a great way to work on UI in a manner isolated from the scene.

#### Deep selection tooling
When working on UI it can be difficult to quickly move to select elements that are nested in the UI, causing your hierarchy to become expanded as you select through, or involving manually searching for you object.  
I have a tool called [NSelection](https://github.com/vertxxyz/NSelection) which allows quick selection of any object under the cursor.
