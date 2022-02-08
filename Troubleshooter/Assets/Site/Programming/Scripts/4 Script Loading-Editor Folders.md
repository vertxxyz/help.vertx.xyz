## Editor folders
### Description
Unity reserves [special folder names](https://docs.unity3d.com/Manual/SpecialFolders.html) that changes the context of the assets contained within them.  
One of these folder names is **Editor**.  

If your script is under a folder named **Editor**—anywhere above it inside the Assets directory—then it is inside of an Editor-only context.  

Having runtime scripts in an editor context is invalid.  

### Resolution
Move the script outside of the context created by the Editor folder. If the folder is not intended to be Editor-only, you should rename it.  

---  
[My script still cannot be loaded.](5%20Script%20Loading.md)