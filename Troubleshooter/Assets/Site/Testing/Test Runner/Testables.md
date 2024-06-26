# Testables

Only tests in [embedded packages](https://docs.unity3d.com/Manual/upm-embed.html) are automatically included by the Test Runner. Embedded packages are folders that are added directly into the `Packages` folder at the root of the project. This does not include local packages, which are referenced by path and live outside the project.

## Resolution
Most packages should not be embedded, instead adding them to your project involves a few steps:

1. Open the **Packages | manifest.json** file.
1. Under any entries like `"dependencies"` and `"scopedRegistries"` add:
   ```json
   "testables": ["com.company.package"]
   ```
   where `com.company.package`{.link--exclude} is the package name that contains your tests.  
   Make sure the previous entry is correctly separated with a comma <kbd>,</kbd>.  
   If you already had a `"testables"` entry, add your package to it like:
   ```json
   "testables": ["com.company.package", "com.company.other-package"]
   ```
1. Close and reopen Unity for the change to be picked up.

---

See [Adding tests to a package](https://docs.unity3d.com/Manual/cus-tests.html) for more information.
