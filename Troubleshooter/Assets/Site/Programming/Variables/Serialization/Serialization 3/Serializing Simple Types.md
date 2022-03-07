## Serializing simple types
### Check the type is correct.
Hover over the type in your IDE to find the namespace[^1], then check the documentation to make sure the type you want to reference actually uses that namespace.  
Common improperly referenced namespaces include: `System`, `System.Numerics`, and `UnityEngine.UIElements`. If you referenced one of these namespaces and do not understand why, it's likely the first place to look!

### Check you are editing the inspected object.
Are you *sure* the inspected object is the type that contains the variable you are declaring? 👀  
Check that your Component or Scriptable Object is the same.  

Using your IDE's Go To Definition[^2] feature should make it clear what type is actually being exposed.  

[^1]: If your IDE is not showing this information, make sure it is [properly configured](../../../IDE%20Configuration.md).  
[^2]: [Visual Studio](https://docs.microsoft.com/en-us/visualstudio/ide/go-to-and-peek-definition?view=vs-2019), [VS Code](https://code.visualstudio.com/Docs/editor/editingevolved#_go-to-definition), [JetBrains Rider](https://www.jetbrains.com/help/rider/Navigation_and_Search__Go_to_Declaration.html). 