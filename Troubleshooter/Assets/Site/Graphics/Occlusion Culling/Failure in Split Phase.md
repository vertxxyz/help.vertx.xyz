# Failure in split phase
In some rare cases, where the scene is vast in size and the smallest occluder parameter has been set to a super small value, Baking may fail with the error "failure in split phase".  
This occurs because the initial step of the bake tries to subdivide the scene into computation tiles.  
The subdivision is based on the smallest occluder parameter and when the scene is humongous in size (like, dozens of kilometers in each direction) too many computation tiles may be created,
resulting in an out of memory error.

## Resolution
Increase the value of smallest occluder and/or split the scene into smaller chunks.
