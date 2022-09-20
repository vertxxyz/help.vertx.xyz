## Floating origin
### Description
The reality of representing real numbers using the limited binary memory of a computer means that these values lose precision as they become larger.  

See [Exposing Floating Point](https://ciechanow.ski/exposing-floating-point/) and the companion site, [float.exposed](https://float.exposed/).

### Resolution
To remedy these issues it's extremely common to reset the coordinate system to the origin as distances increase. Large-scale spaces are often scaled down to make reasonably travelled distances be inside a certain range of precision.

If your object is far from the origin, consider regularly resetting the world coordinates to be close to the origin. The term for this is "floating origin".  
If your object is extremely large, consider scaling it down to a more reasonable scale. Game development is full of compromises like these to avoid limitations imposed by computers and real-time rendering.