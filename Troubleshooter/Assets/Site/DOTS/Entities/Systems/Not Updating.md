# Systems not updating
Use the [Systems window](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/manual/editor-systems-window.html) to check the Relationships of any Queries associated with the system.

Make sure the system is not greyed out in the Systems window, this means it has been disabled using `.Enabled = false`.

## The system contains a required query with no results
1. If your EntityQuery should match against disabled components make sure to include [`.WithOptions(EntityQueryOptions.IncludeDisabledEntities)`](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.EntityQueryBuilder.WithOptions.html).
1. If your EntityQuery should match against components on Systems make sure to include [`.WithOptions(EntityQueryOptions.IncludeSystems)`](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.EntityQueryBuilder.WithOptions.html).
1. If your EntityQuery should match against components on Prefabs make sure to include [`.WithOptions(EntityQueryOptions.IncludePrefab)`](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html?subfolder=/api/Unity.Entities.EntityQueryBuilder.WithOptions.html).
1. Search for components contained in the query using the Components window (**Window | Entities | Components**), if there is no result then you have not added the component to an entity.

---

This list is not exhaustive, please <<report-issue.html>> so this page can be improved if you have additions.  
