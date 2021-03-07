### Null Reference Exception
#### Description
A `NullReferenceException` happens when your script code tries to use a variable which isn’t set, or found. It's probably just unassigned in the inspector.  
The error tells you this, and also gives you the line where the error is occurring, so you can go to that line and narrow down what is missing.  
*Declaring* a reference variable does not automatically assign it an appropriate value.  

#### Resolution

- Assign a reference to the variable
    - Drag and drop the reference **in the inspector** (if it is a serialized)
    - Manually assign the reference in a method like Awake or Start
- Check whether the reference is not null before trying to access it  
    `if(example != null)`
  
Additionally ensure that nothing is destroying the Object, or setting it to `null` before you attempt to use it.  
When troubleshooting which variable is null, be aware that only reference types can be null. Ie. `class`, `interface`, or `delegate` variables.

---

See [General Debugging](../../Debugging.md) for troubleshooting techniques.  
See [Serializing Component References](../../Variables/Other%20Members/Serializing%20Component%20References.md) to learn how to assign variables in the Inspector.