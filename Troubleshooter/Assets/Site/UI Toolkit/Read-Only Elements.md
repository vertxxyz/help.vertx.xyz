# Read-only elements
VisualElements are defined in code, and any sub-elements created by the class in its constructor are shown as read-only (disabled) in the UI Builder.

Through the UI Builder and UXML you can only add children to that element's content container, a child element defined by the class.

## Editing the style of a read-only element
You cannot add inline styles to read-only elements, use [USS selectors](Selectors.md) to style the element.  

To style an element globally, target its classes with a simple selector.

To style an element locally, create a complex selector targeting from the parent element you can add a name or custom class to.
