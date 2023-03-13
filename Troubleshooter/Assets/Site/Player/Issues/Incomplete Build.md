## Player: Incomplete build
### Issue
A complete Windows Unity build is made up of all the files and folders that build creates. A build is not just an `exe` file.  
Depending on the type of build this can include a `_Data` folder, and a number of `dll` and `exe` files.

### Resolution
When moving, uploading, or publishing your build keep all the files and folders in the build folder. Only folders suffixed with `_DoNotShip` or `_ButDontShipItWithYourGame` should remain unpublished.

### Extra details
The `exe` file for all Unity games published with a version of Unity are identical. They all just open and manage the build in their folder that matches their name.