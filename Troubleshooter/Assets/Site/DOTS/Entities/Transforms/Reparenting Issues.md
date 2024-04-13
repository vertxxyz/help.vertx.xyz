# Entity Transforms: Reparenting issues
For a `LocalTransform` to truly become a child of an entity both the `Parent` component and `Child` buffer must be set correctly. As a user, you should only be modifying `Parent`, which should automatically propagate.

This can fail to be set via various ways:
- Setting or copying the `PreviousParent` to be the same as the `Parent` yourself. Do not modify `PreviousParent`.
- Setting `Parent` in a way that bypasses versioning/change checks.
