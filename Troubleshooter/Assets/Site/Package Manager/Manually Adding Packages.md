### Manually Adding Packages
Packages in a project are described by the `manifest.json` file in the *Packages* folder at the root of a project.  
To manually add a package, find its registered package name, and a version of the package you know exists.  

##### Example
1. For the 2D Pixel Perfect package, you would navigate to its [documentation](https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@latest/)[^1], and note the url contains `com.unity.2d.pixel-perfect`.  
    A package name is always in this format, `<domain-name-extension>.<company-name>.<package-name>`.  
    Some packages will have installation instructions in their documentation, and it's important to exhaust that information first.  
2. Switch to the changelog tab, and note a release number, e.g. `5.0.1`. This number may contain a suffix, e.g. `5.0.0-pre.2`.  
3. Open the `manifest.json` file, and append a comma to the last line inside the dependencies section.
4. Add your package and release number to the end in the same format as other entries. E.g. `"com.unity.2d.pixel-perfect": "5.0.1"`.  
    It is important to note that the last entry should not have a trailing comma. This is invalid JSON and Unity will fail to load the manifest.  

---  

See [Package Manifest](https://docs.unity3d.com/Manual/upm-manifestPkg.html) for more information.  

[^1]: You can find the documentation for packages by looking in the [packages by keywords](https://docs.unity3d.com/Manual/pack-keys.html) section of the Unity Manual.