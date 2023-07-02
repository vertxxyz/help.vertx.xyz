<<Abbreviations/UPM.md>>
## <img src="/Images/visualstudio.svg" alt="visual studio" style="vertical-align:middle; margin:0 6px 0 0; width:32px; height:32px"> Visual Studio configuration
### Installed with Unity Hub
Visual Studio will have the Unity workload installed.  
Follow the [configuration instructions](https://docs.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity#configure-unity-to-use-visual-studio) to configure the remaining setup.  

### Installed manually
Follow the [configuration instructions](https://docs.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity#install-unity-support-for-visual-studio), including installation of the Unity workload.

### Installing for the first time
::::note  
#### Install Visual Studio via the Unity Hub
Installing Visual Studio via the Unity Hub means it's mostly ready for use with Unity.

:::note
#### No Unity versions are installed
Tick _Microsoft Visual Studio Community_ during the installation process in the Unity Hub. (**Installs | Install Editor**)  
:::  
:::note  
#### A Unity version is installed
1. Navigate to the Installs page in the Unity Hub and click the cog ⚙️ icon on an install.
1. Select **Add Modules**.  
   ^^^
   ![Add Modules](../../Unity%20Hub/add-modules.png)
   ^^^ Add modules in the Unity Hub
1. Tick _Microsoft Visual Studio Community_.
1. Select **Install**.

:::  
Follow the [configuration instructions](https://docs.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity#configure-unity-to-use-visual-studio) to check the remaining setup.  
::::  
::::note  
#### I want to install Visual Studio manually
Follow all of the instructions listed on [this page](https://docs.microsoft.com/en-us/visualstudio/gamedev/unity/get-started/getting-started-with-visual-studio-tools-for-unity).  
::::

---  
Ensure the [Visual Studio Editor](https://docs.unity3d.com/Manual/com.unity.ide.visualstudio.html) package is installed and updated in UPM (`com.unity.ide.visualstudio`).  