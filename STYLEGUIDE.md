# Style Guide

Please read the [contributing info](CONTRIBUTING.md) for more a more technical project overview.

This guide does not describe how to write documentation or code. It only lists style restrictions used by this site's pages.  
The guide to written documentation used by this site is the [Microsoft Writing Style Guide](https://docs.microsoft.com/en-us/style-guide/welcome/).  
You may find plenty of examples that don't follow that style, I'm learning how to write quality documentation too!

## General

All pages should have 2nd level header as a title.
```md
## Title
```

Bold important information that when skimmed could be easily missed or ignored, this can include qualifiers and negations.  
```md
Examples A and B **do not** function with C.
```

When information is vital and could still be skimmed it should be highlighted with an error, warning, or info block.

```md
:::warning
Examples A and B **do not** function with C.
:::
```

Blocks can be made smaller when desirable, making them fit better against or within paragraphs.
```md
:::warning{.inline}
This is a much smaller warning.
:::
```

When using abbreviations, embed the expanded abbreviation into the page (present in the Assets/Embeds/Abbreviations directory.)

```md
<<Abbreviations/NRE.md>>
## Missing references
This example can throw an NRE, causing code execution to halt.
```

Sidebar links should not contain periods.
```md
### Example Sidebar
- [External example site link](https://example.com)
```

Periods should be inside links if the link is a major part of the sentence, but after links if they are not.
```md
[Sentence containing an example link.](link.md)  
Sentence containing an example [link](link.md).
```

## Pages
### Hub Pages
When a hub page has many different topics that are branched off as links within sentences please use dot points to distinguish each line.  
If a page is likely going to become something that uses this format, also use this style.  
```md
- [Example sentence that has a link.](link0.md)
- [More example sentences that contain links.](link1.md)
- [Even more example sentences that contain links.](link2.md)
```

### General Pages
When a page contains a description and links that continue the line of troubleshooting, the links should be separated from the content with a horizontal rule.  
```md
Example description sentence containing a line of troubleshooting that might solve a problem.  

---  
- [Option A.](a.md)
- [Option B.](b.md)
```

When a page is made up of a description of a problem, and a resolution, add 3rd level headers in this style that separate them into two paragraphs with limited overlap.
```md
### Description
This error is describing this problem because of this reasoning.  

### Resolution
These are the instructions that would solve the issue if that was the case.
```

Common alternative or additional titles include: **Implementation**, **Notes**.

## Tables
Tables with a vertical 'header' column should contain bold text in their entry. Avoid using bold text in the left column otherwise. If you require highlighting info, consider emoji!

```md
| Header     | Header | Header |
|------------|--------|--------|
| **Header** | Body   | Body   |
| **Header** | Body   | Body   |
```

## Miscellaneous
### Keyboard Shortcuts
```md
Hold <kbd>Ctrl</kbd> while left clicking.
Press <kbd>Alt+Ctrl+F</kbd>.
```  
### Menu Paths or Hierarchical Locations
```md
**File | Settings | Plugins**
```  
### References to packages
Packages should be referenced by display and internal names.
```md
[JetBrains Rider Editor](https://docs.unity3d.com/Manual/com.unity.ide.rider.html) (`com.unity.ide.rider`)
```
Packages should be linked to by `@latest`, not by a strict version. You can link to subfolders of a manual page using the syntax: `@latest/index.html?subfolder=/manual`, swap `manual` with the appropriate subfolder, `api` for example.