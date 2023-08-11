## Testables

Only tests in embedded packages are automatically included by the Test Runner. Embedded packages are folders that are added directly into the `Packages` folder at the root of the project. This does not include local packages, which are referenced by path and live outside the project.

### Resolution
Most packages should not be embedded, instead adding them to your project involves a few steps:

1. Open the **Packages | manifest.json** file.
2. Under any entries like `"dependencies"` and `"scopedRegistries"` add:
   ```json
   "testables": ["com.unity.some-package"]
   ```
   where `com.unity.some-package` is the package name that contains your tests.  
   Make sure the previous entry is correctly separated with a comma <kbd>,</kbd>. If you already had a `"testables"` entry, add your package to it like `["com.unity.some-package", "com.unity.other-package"]`.
3. Close and reopen Unity for the change to be picked up.

---

See [Adding tests to a package](https://docs.unity3d.com/Manual/cus-tests.html) for more information.