## HDR values

Not all Color fields will support HDR values. If you're using a field that you do not control the code of and it does not have a HDR color picker, then it's unlikely it was ever intended to take HDR values.

### Resolution

Mark the field with the [ColorUsageAttribute](https://docs.unity3d.com/ScriptReference/ColorUsageAttribute.html), with `hdr` enabled:  

<<Code/Specific/ColorUsageAttribute.rtf>>