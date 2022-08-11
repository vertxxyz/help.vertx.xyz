# Style Guide

This guide does not describe how to write documentation or code. It only lists style restrictions used by this site's pages.  
The guide to written documentation used by this site is the [Microsoft Writing Style Guide](https://docs.microsoft.com/en-us/style-guide/welcome/).  
You may find plenty of examples that don't follow that style, I'm learning how to write quality documentation too!

## General

All pages should have 2nd level header as a title.
```
## Title
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
## Missing references
This example can throw an NRE, causing code execution to halt.
```

Sidebar links should not contain periods.
```
### Example Sidebar
- [External example site link](https://example.com)
```

Periods should be inside links if the link is a major part of the sentence, but after links if they are not.
```
[Sentence containing an example link.](link.md)  
Sentence containing an example [link](link.md).
```

## Pages
### Hub Pages
When a hub page has many different topics that are branched off as links within sentences please use dot points to distinguish each line.  
If a page is likely going to become something that uses this format, also use this style.  
```
- [Example sentence that has a link.](link0.md)
- [More example sentences that contain links.](link1.md)
- [Even more example sentences that contain links.](link2.md)
```

### General Pages
When a page contains a description and links that continue the line of troubleshooting, the links should be separated from the content with a horizontal rule.  
```
Example description sentence containing a line of troubleshooting that might solve a problem.  

---  
- [Option A.](a.md)
- [Option B.](b.md)
```

When a page is made up of a description of a problem, and a resolution, add 3rd level headers in this style that separate them into two paragraphs with limited overlap.
```
### Description
This error is describing this problem because of this reasoning.  

### Resolution
These are the instructions that would solve the issue if that was the case.
```

## Miscellaneous
### Keyboard Shortcuts
```
Hold <kbd>Ctrl</kbd> while left clicking.
Press <kbd>Alt+Ctrl+F</kbd>.
```  
### Menu Paths or Hierarchical Locations
```
**File | Settings | Plugins**
```  