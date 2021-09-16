### Resource Paths
#### Description
Paths used by the Resources API are **local to the Resources folder** above the asset.  
Paths **do not include the file extension**.  
Paths **cannot contain back-slashes**.  
See [Resources.Load](https://docs.unity3d.com/ScriptReference/Resources.Load.html).

#### Resolution
Example:
##### Incorrect
`"Assets/Resources/Missiles/PlasmaGun.prefab"`

##### Correct

`"Missiles/PlasmaGun"`