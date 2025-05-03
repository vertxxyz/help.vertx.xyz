# Physics queries: Visual debugging
## Physics debugger
Use the [Physics Debugger](../Debugging/Physics%20Debugger.md) to visualise your queries in the Scene view.

^^^
![Physics Debugger showing queries](../Debugging/physics-debugger.png)
^^^ Physics Debugger showing queries in the Scene view

## Debugging functions
Alternatively you can use a [visual debugging](../Debugging/Visual%20Debugging.md) API to draw these queries, which can create more persistent or elaborate visualisations.  
:::info  
Be particularly cautious to provide the exact same values you use in your query.
:::  

## Common mistakes resolved by visual debugging
Watch out for these common mistakes:

- Using a **position** in place of an input requiring a **direction**.
- Using a position or direction from a different object than expected.
- Using a position or direction in the wrong space. Queries usually made in world space.
