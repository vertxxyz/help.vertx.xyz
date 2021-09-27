## Unassigned Reference Exception
An Unassigned Reference Exception is a type [Null Reference Exception](Null%20Reference%20Exception.md) where a Unity Object field has not been assigned to, and the contents do 
not exist.  

If it appears the component has the field assigned in the Inspector, search the scene for the component type with `t:ExampleComponent`, ensuring there aren't other unexpected instances causing the issue.