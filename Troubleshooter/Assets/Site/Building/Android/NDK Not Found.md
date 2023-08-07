## Android NDK not found
```
Android NDK not found or invalid. Please, fix it in Edit / Unity -> Preferences -> External tools
```

### Resolution
#### If the NDK has not been installed
1. Navigate to the Unity Hub and [install the Android SDK & NDK Tools module](../../Unity%20Hub/Module%20Installation.md) under Android Build Support.  
   Make sure that the Unity version you install the module for is the version you are using.
1. Navigate to **Edit | Preferences | External Tools | Android** in Unity and tick **Android NDK Install with Unity**.

#### If the NDK was already installed
1. Open [Android Studio](https://developer.android.com/studio) (install it if you do not have it).
1. Select **⚙️ Configure** from the bottom right of the welcome page.
1. Select **SDK Manager**.
1. Navigate to **Appearance & Behaviour | System Settings | Android SDK | SDK Tools** and tick the **Show Package Details** checkbox from the bottom right.
1. Scroll down to **NDK (Side by side)** and install the latest version specified by the [supported dependency version](https://docs.unity3d.com/Manual/android-sdksetup.html#supported-dependency-versions) for your Unity version.  
   If you are confused by the dependency version syntax, cross-reference it with the `ndkVersion` listed on [this page](https://github.com/android/ndk/wiki/Unsupported-Downloads).
1. Navigate to **Edit | Preferences | External Tools | Android** in Unity and manually browse **Android NDK Install with Unity** to the installation directory for the NDK you installed (**ndk | version ** under your SDK install).


---

If you find an unlisted resolution please <<report-issue.html>> so this page can be improved.  