# SerializeReference

By default variables marked with `SerializeReference` will not show unless they have already been assigned.  
Unity provides no drawer for `null`, and does not show an object picker in this circumstance. 

## Resolution
### Assign a variable manually
Variables initialised via code or via editor scripts should appear correctly if they are [properly serializable](Custom%20Types.md).  

### Assign the variable via Editor UI
Either write your own editor code, or use a custom property drawer like [Vertx.SerializeReferenceDropdown](https://github.com/vertxxyz/Vertx.SerializeReferenceDropdown) to pick an instance of a type from the editor.
