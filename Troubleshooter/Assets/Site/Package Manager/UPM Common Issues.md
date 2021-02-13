### Common Package Manager Issues
#### Package Manager Network Troubleshooting
There's UPM network troubleshooting resources [here](https://docs.unity3d.com/Manual/upm-network.html), and there's a pinned post at the top of [the UPM forum](https://forum.unity.com/forums/package-manager.150/).  
Make sure you've followed all those instructions if you're having issues, and then post on the forums or contribute to a pre-existing post there.

#### Finding Packages
If a package is not visible, it may be In Preview. Preview packages are not verified for safe use with your version of Unity. You can find out how to enable preview packages with the current version of Unity [here](https://docs.unity3d.com/Manual/pack-preview.html).  
Before 2020 this was in the Advanced dropdown at the top of UPM, but it has now moved to Project Settings (*Edit > Project Settings*.)  
As of 2020 the visibility of some packages in UPM has been removed. This is because they are deemed far from being verified, or are only used as a dependency to other packages. You can find more about this [here](https://forum.unity.com/threads/visibility-changes-for-preview-packages-in-2020-1.910880/).

#### Packages From Git
Git packages can be [added via using their git link](https://docs.unity3d.com/Manual/upm-git.html), this is the url ending in `.git`, and not the url of the repository page.  
As of 2019.3.4.f1 (and 2020.1.a21) git urls for packages [supports subfolders](Common%20Issues/Git%20Subfolders.md).

🚧 Under Construction 🚧