# Package errors

Packages are read-only addons or features that are versioned, with different versions having different compatibility with Unity releases and other packages. Errors within packages are common due to the complexity of these compatibility issues.

## Resolution
If a package has errors, either:
1. The package isn't compatible with your Unity release.
2. The package isn't compatible with another package.
3. The package is corrupted.

The general solution to all 3 problems is to upgrade or downgrade the package until a version has compatibility with your project.

### Upgrading or downgrading
:::info{.small}
Always ensure you have a backup before making project changes.
:::

You can [upgrade or downgrade versions](https://docs.unity3d.com/Manual/upm-ui-update.html) to match your release.  
Some versions may not be visible in the package manager due to Unity restricting their visibility. When this happens you can manually edit the version through the `manifest.json` file in the **Packages** directory at the root of the project.  

When making changes to packages, prioritise newer versions of packages. New versions have fixes and improves features. Downgrade to older versions when new ones are not compatible, or contain regressions or broken features.

### Version compatibility
To get a better indication of version compatibility you should check the **changelog** tab in the documentation[^1]. Versions are in the header for each release, e.g. `5.0.1`. This number may contain a suffix, e.g. `5.0.0-pre.2`.  
:::warning{.small}
Not all versions are compatible with all Unity releases.
:::

### Reacquiring corrupted packages
Packages are cached globally in:

^^^

| OS                            | Directory                        |
|-------------------------------|----------------------------------|
| Windows (user account)        | 	`%LOCALAPPDATA%\Unity\cache`    |
| Windows (system user account) | 	`%ALLUSERSPROFILE%\Unity\cache` |
| macOS                         | 	`$HOME/Library/Unity/cache`     |
| Linux                         | 	`$HOME/.config/unity3d/cache`   |
^^^ You can see this table in the [documentation](https://docs.unity3d.com/Manual/upm-cache.html).


Packages are also copied locally to projects in their **Library/PackageCache** folder.

Close Unity, remove the package from both locations, and reopen Unity so it is forced to reacquire the package from scratch. Certain built-in packages are also downloaded with the editor, reinstalling the editor is the best way to ensure they're validated.

[^1]: You can find the documentation for packages by looking in the [packages by keywords](https://docs.unity3d.com/Manual/pack-keys.html) section of the Unity Manual.
