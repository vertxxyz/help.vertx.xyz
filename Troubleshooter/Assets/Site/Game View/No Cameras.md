# No Cameras rendering

Generally, if there are no cameras rendering to the Game view this is unwanted behaviour, so there is an Editor-only warning to help debug this situation.

## Resolution
### I do not want any cameras rendering
Right-click on the Game view tab and uncheck **warn if no cameras rendering** to remove the warning.

### I want cameras rendering
For the warning, a camera must be active and enabled to be considered rendering.
Rendering a camera via code does not count.
If you are rendering cameras manually, see above.
