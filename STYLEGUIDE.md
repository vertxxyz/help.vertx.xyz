# Style Guide

This guide does not describe how to write documentation or code. It only lists style restrictions used by this site's pages.  
The guide to written documentation used by this site is the [Microsoft Writing Style Guide](https://docs.microsoft.com/en-us/style-guide/welcome/).  
You may find plenty of examples that don't follow that style, I'm learning how to write quality documentation too!

## General

All pages should have 3rd level header as a title.
```
### Title
```

Bold important information that when skimmed could be easily missed or ignored, this can include qualifiers and negations.  
```
Examples A and B **do not** function with C.
```

When information is vital and could still be skimmed it should be highlighted with a error/warning/info block.

```
:::warning
Examples A and B **do not** function with C.
:::
```

When using abbreviations, embed the expanded abbreviation into the page (present in the Assets/Embeds/Abbreviations directory.)

```
<<Abbreviations/NRE.md>>
### Missing References
This example can throw an NRE, causing code execution to halt.
```

Sentences that are entirely links do not need full stops, but sentences that are partially links do, if these are mixed the full stop should always be present.  
Full stops should not be a part of the link. Clarifications in parentheses are not seen as a part of the sentences when it comes to linking.
```
#### Example 1
[A sentence that is a link](A.md)  
[Another sentence that is a link](A.md)  
#### Example 2
[A sentence that is a link](A.md).  
Another sentence that [has a link](A.md).  
#### Example 3
[A sentence that is a link](A.md)  
[Another sentence that is a link](A.md) (Clarification)  
```

## Pages
### Hub Pages
When a hub page has many different topics that are branched off as links within sentences please use dot points to distinguish each line.  
If a page is likely going to become something that uses this format, also use this style.  
```
- [Example sentence that has a link](link0.md)  
- [More example sentences that contain links](link1.md)  
- [Even more example sentences that contain links](link2.md)  
```

When a page has a links made up of few **related** options, the dot points should be omitted.  
```
[Either A](a.md)  
[Or B](b.md)  
```

If a page is a hub for different topics that are distinguished with headers, the dot points should also be omitted.

```
#### Example 1
[A](a.md)  
[B](b.md)  
#### Example 2
[C](c.md)  
[D](d.md)  
```

### General Pages
When a page contains a description and links that continue the line of troubleshooting, the links should be separated from the content with a horizontal rule.  
```
Example description sentence containing a line of troubleshooting that might solve a problem.  

---  
[Option A](a.md)  
[Option B](b.md)  
```

When a page is made up of a description of a problem, and another description of a resolution, add 4th level headers in this style that separate them into two paragraphs with limited overlap.
```
#### Description
This error is describing this problem because of this reasoning.  

#### Resolution
These are the instructions that would solve the issue if that was the case.
```
