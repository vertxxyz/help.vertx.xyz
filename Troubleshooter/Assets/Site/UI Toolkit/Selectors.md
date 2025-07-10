---
title: "USS selectors"
description: "Learn the UI Toolkit debuggers, and how to resolve performance issues."
---
# USS selectors
## Learn selector rules
Go through the [USS selectors](https://docs.unity3d.com/Manual/UIE-USS-Selectors.html) documentation to learn how to construct USS queries against your hierarchy.  
### Examples
This example lists selectors commonly used for basic styling.

^^^
```css
/* This selector (a word without a prefix) matches types directly.
   This example will match any Button. */
Button { ... }

/* This selector "#" matches elements by name.
   This example will match any elements with the name "example". */
#example { ... }

/* This selector "." will match elements using a USS class.
   This example will match any elements with the "example" class. */
.example { ... }

/* This selector ">" combines two other selectors, matching the second if a direct child of the first in the hierarchy.
   This example will match any elements with the "child" class that are directly below elements with the "example" class.
   If the element also has a contentContainer set then the direct children of that container are also matched. */
.example > .child { ... }

/* This selector " " combines two other selectors, matching the second if it's below the first in the hierarchy.
   This example will match any elements with the "descendant" class that are below elements with the "example" class. */
.example .descendant { ... }

/* This selector ":" matches elements in certain states. 
   This example matches hovered buttons. See the complete list in the caption below this code block. */
Button:hover { ... }

/* You can combine selectors by listing them without a space. This only works when each selector is clearly distinguished with # or . separating the list.
   This example matches any Button elements with the name and class "example" */
Button#example.example { ... }

/* You can apply the same styling rules to multiple groups of selectors by separating them with commas.
   This example matches any Button, and anything with the "example" class, or name. */
Button, .example, #example { ... }

/* The universal selector targets everything. Generally avoid it by preferring specifics. */
* { ... }

```
^^^ See the [complete list of supported pseudo-classes](https://docs.unity3d.com/Manual/UIE-USS-Selectors-Pseudo-Classes.html).

## Understand USS precedence
There are a few levels to how styling is prioritised.  
1. Understand [selector precedence](https://docs.unity3d.com/Manual/UIE-uss-selector-precedence.html).  
2. If you are using [Theme Style Sheet](https://docs.unity3d.com/Manual/UIE-tss.html), note that style sheets lower in the list are applied over those above.

Inline styles override all USS styles.

## Selector choice

To style an element globally, target its classes with a simple selector.

To style an element locally, create a compound selector targeting from the parent element you can add a name or custom class to.

Make sure the [USS precedence](#understand-uss-precedence) overrules existing selectors.
