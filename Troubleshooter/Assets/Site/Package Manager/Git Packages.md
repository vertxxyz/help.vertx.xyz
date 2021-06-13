### Git Packages
#### Description
Git packages can be [added via using their git link](https://docs.unity3d.com/Manual/upm-git.html), this is the url ending in `.git`, and not the url of the repository page.  

#### Open UPM
Sometimes these packages are hosted on [Open UPM](https://openupm.com), in these cases you can follow [these steps](Open%20UPM%20Packages.md) to easily add and maintain packages to your project.

#### Subfolders
As of 2019.3.4.f1 (and 2020.1.a21) git urls for packages [supports subfolders](Git%20Subfolders.md).

#### Updating Packages
To update a git package with new changes, remove the lock from the `packages-lock.json` file.  
You can find this file in your Packages folder at the root of your project. The lock is the block of json that relates to your package.  
If you do not have a `packages-lock.json` file, then the `manifest.json` file should contain the lock near the bottom of the json.