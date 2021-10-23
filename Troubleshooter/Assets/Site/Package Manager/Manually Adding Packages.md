## Manually adding packages

To manually add a package, find its registered package name, and a version of the package you know exists.  

:::note
#### Finding a package name
Navigate to the documentation for a package[^1], and note the package name in the URL.  
A package name is always in this format, `<domain-name-extension>.<company-name>.<package-name>`.  
For example, the [2D Pixel Perfect package](https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@latest/) contains `com.unity.2d.pixel-perfect` in its URL.  
Some packages will have installation instructions in their documentation, and it's important to exhaust that information first.  

#### Finding package versions
In the documentation's **Changelog** tab versions are in the header for each release, e.g. `5.0.1`. This number may contain a suffix, e.g. `5.0.0-pre.2`.  
Not all versions are compatible with all Unity releases. You may have to upgrade or downgrade versions to match your release.  
:::  

### Adding packages by name
1. Open the Package Manager (**Window | Package Manager**).
2. Select **+** from the top left.
3. Select **Add package by name** or **Add package from git URL** if by name is not present.
4. Enter the package name.  
   You can specify a version with the format `com.company.package@0.0.0`, this is optional.
5. Select **Add**.

### Adding packages via the manifest
Packages in a project are described by the `manifest.json` file in the **Packages** folder at the root of a project.

#### Example
1. Open the manifest in a text editor, and append a comma to the last line inside the `dependencies` section.
2. Add your package and release number to a new line in that section in the same format as other entries. E.g. `"com.unity.2d.pixel-perfect": "5.0.1"`.  
    It is important to note that the last entry should not have a trailing comma. This is invalid JSON and Unity will fail to load the manifest.  
3. Save the manifest and return to Unity.

See [Package Manifest](https://docs.unity3d.com/Manual/upm-manifestPkg.html) for more information.  

[^1]: You can find the documentation for packages by looking in the [packages by keywords](https://docs.unity3d.com/Manual/pack-keys.html) section of the Unity Manual.