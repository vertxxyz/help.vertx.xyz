### Description
Defining a serialized field in a class scope allows that instance can be referred to elsewhere in the class.

### Resolution

The field must be marked with [SerializeField](https://docs.unity3d.com/ScriptReference/SerializeField.html):  

<<Code/Variables/Serialized Reference.rtf>>  

**or** can be `public`:  
<<Code/Variables/Public Reference.rtf>>

:::note
The example uses the `Transform` type, it will need to be replaced with the type intended to be referenced.
:::  

The instance the variable is pointing to is defined in the Inspector.  