## Baking Transform components
The final transform components are driven by calls to `GetEntity`.
`GetEntity` takes a `TransformUsageFlags` which defines which components are added to objects. If you manually add components, unless the provided flags are `TransformUsageFlags.ManualOverride`, they will be removed if they don't match the setup.

### Resolution
If you are missing a component, make sure you are using a `TransformUsageFlags` flag with `GetEntity` that will include it (or use `ManualOverride` and manually add the components you need).
#### Notes
- A GameObject's [static flags](Static%20Entities.md) may override flags, which in turn overrides the final components.
- Prefabs retrieved by `GetEntity` are given the `Dynamic` flag by default.

### Table
This table shows the outcome if you only specified a single `TransformUsageFlags` flag with one baker.  
- **Green** areas mean the component will be added.  
- **Orange** areas mean the component may be added based on what's specified.  
- **Unshaded** areas mean the component will not be added.

^^^

|                     | LocalTransform       | LocalToWorld         | PostTransformMatrix                                | Parent                              | Child                             |
|---------------------|----------------------|----------------------|----------------------------------------------------|-------------------------------------|-----------------------------------|
| **None**            | N                    | N                    | N                                                  | N                                   | N                                 |
| **Renderable**      | M Has dynamic parent | Y                    | M Has non-uniform scale<br/>and has dynamic parent | M Has dynamic parent                | M Has dynamic child               |
| **Dynamic**         | Y                    | Y                    | M Has non-uniform scale                            | M Has dynamic parent                | M Has dynamic child               |
| **WorldSpace**      | N                    | Y                    | N                                                  | N                                   | M Has dynamic child               |
| **NonUniformScale** | M Has dynamic parent | Y                    | Y                                                  | M Has dynamic parent                | M Has dynamic child               |
^^^ ::Note that [static flags](Static%20Entities.md) may override your flags and prefabs retrieved via `GetEntity` are given the `Dynamic` flag by default.::{.warning}

### Combined flags
Multiple bakers will combine their flags, specifying all that is needed, and in some cases this can be quite complicated.  
For example if a `Renderer` with one submesh is present, then the transform will gain a `Renderable` tag. If there are multiple submeshes then additional entities will be generated, and the root entity will not get flagged as `Renderable`.