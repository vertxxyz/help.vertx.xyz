# Resource paths
Guidelines for paths used by the Resources API:
- Paths are **local to the Resources folder** above the asset.  
- **Don't include the file extension**.  
- **Don't use back-slashes**.

See [`Resources.Load`](https://docs.unity3d.com/ScriptReference/Resources.Load.html) for more usage information.

## Example

```
Assets/
├── ...
├── Resources/
│   ├── Missiles/
│   │   ├── PlasmaGun.prefab
│   │   └── ...
│   └── ...
└── ...
```

```csharp
// 🟢 Correct
var plasmaGun = Resources.Load<GameObject>("Missiles/PlasmaGun");

// 🔴 Incorrect
var plasmaGun = Resources.Load<GameObject>("Assets/Resources/Missiles/PlasmaGun.prefab");
```
