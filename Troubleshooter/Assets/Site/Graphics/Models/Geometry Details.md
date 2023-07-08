## Geometry details in Unity
Different applications store vertex attributes in different ways.

Unity needs separate verts for different attributes. Which can be colours, normal information for smoothness/hard edges, UVs, and so on.  
Other applications may store this information as a singular merged vertex, and so your geometry details may be off between applications.

Unity also requires geometry be triangulated, creating more vertices in Unity than were displayed by your modelling application.