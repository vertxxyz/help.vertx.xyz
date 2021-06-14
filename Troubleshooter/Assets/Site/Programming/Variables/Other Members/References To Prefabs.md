### Referencing Prefabs From Scenes
<<Variables/Serialized Prefab References.md>>
Ensure you have dragged the prefab asset into the slot or you will get a [Null Reference Exception](../../Common%20Errors/Runtime%20Exceptions/Null%20Reference%20Exception.md) (Unassigned Reference Exception).  

<video width="750" height="200" autoplay loop muted><source type="video/webm" src="https://help.vertx.xyz/Video/prefab-references.webm"></video>

When cloning a GameObject or Component using `Instantiate`, all child objects and components are also cloned with their properties set like those of the original object.  
This means you should reference a root component on a prefab asset.  

<<Variables/Further Info.md>>