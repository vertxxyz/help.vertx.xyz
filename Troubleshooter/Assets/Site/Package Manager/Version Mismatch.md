# Package version mismatch
Unity 6 introduces a change to built-in dependency resolution:

> **Package Manager**: Dependency resolution no longer breaks when there is a dependency on a core package with a different version. Instead, the Package Manager will use the built-in package version.  
> Alternatively, you can preserve the previous behavior and override specific core packages with a version pulled from a custom scoped registry by setting the following scoped registry configuration: `"overrideBuiltIns": true`.

## Example
If you want to reference a specific version of `com.unity.ugui`, but another package depends on the built-in version, then the version referenced by the manifest will be silently overruled.

To override this behaviour navigate to your `Packages/manifest.json` file, and add an entry to the `scopedRegistries` array with `"overrideBuiltIns": "true"`:

^^^
```json
"scopedRegistries": [
    {
      "name": "Override Unity Built-In",
      "url": "https://packages.unity.com",
      "overrideBuiltIns": "true",
      "scopes": [
        "com.unity.ugui"
      ]
    }
]
```
^^^ Add the registry via the Package Manager section in Project Settings, modifying the result with the `overrideBuiltIns` line if unfamiliar with json.

:::info
Adding `com.unity.ugui` as the scope will override the built-in behaviour for that specific package.
:::
