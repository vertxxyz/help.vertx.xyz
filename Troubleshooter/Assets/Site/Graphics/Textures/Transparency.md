### Transparency Settings
#### Description
Unity will not import textures as transparent by default.

#### Resolution
First, ensure that your source texture is actually transparent when viewed via another program.  
Next, ensure the Alpha Source is set to your alpha input, and Alpha Is Transparency is ticked.

![Transparency Options](transparency-options.png)  

##### For use in 2D / UI
2D and UI textures should be imported as Sprites. This should automatically set the above options when valid.

![Texture Type - Sprite](texture-type-sprite.png)  