## Styling elements in UI Toolkit

This guide describes how to create USS selectors to style complex elements.  

To do this, you must learn USS. USS is a query system similar to CSS that allows you to construct rules to constrain styling to specific elements.  

We choose not to use direct inline styles via C#. Inline styles override all other styling, require recompilation to adjust, and must be specified every time an element is created.

::::note
#### Steps taken when styling an element
- Learn selector rules to target classes below objects in the hierarchy.
- Understand USS precedence.
- Create your element so it's ready for styling.
  + Learn how to name classes.
- Use the UI Toolkit Debugger to inspect your element.
  + Preview style adjustments of your elements.
- Construct a USS rule that targets your element.

#### Additionally
- Take a peek at the C# source code that actually constructed your control.

::::

[//]: # ([TOC]  )

### Learn selector rules
Go through the [USS selectors](https://docs.unity3d.com/Manual/UIE-USS-Selectors.html) documentation to learn how to construct uss queries against your hierarchy.  
#### Examples
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
   This example will match any elements with the "child" class that are directly below elements with the "example" class. */
.example > .child { ... }

/* This selector " " combines two other selectors, matching the second if it's below the first in the hierarchy.
   This example will match any elements with the "descendant" class that are below elements with the "example" class. */
.example .descendant { ... }

/* This selector ":" matches elements in certain states. 
   This example matches hovered buttons. See the complete list in the caption below this code block. */
Button:hover { ... }

/* Collapsable: Rarer selectors */
/* Rarer selectors */
/* You can combine selectors by listing them without a space. This only works when each selector is clearly distinguished with # or . separating the list.
   This example matches any Button elements with the name and class "example" */
Button#example.example { ... }

/* You can apply the same styling rules to multiple groups of selectors by separating them with commas.
   This example matches any Button, and anything with the "example" class, or name. */
Button, .example, #example { ... }

/* The universal selector targets everything. Generally avoid it by preferring specifics. */
* { ... }

/* End Collapsable */
```
^^^ See the [complete list of supported pseudo-classes](https://docs.unity3d.com/Manual/UIE-USS-Selectors-Pseudo-Classes.html).

### Understand USS precedence
There are a few levels to how styling is prioritised.  
1. Understand [selector precedence](https://docs.unity3d.com/Manual/UIE-uss-selector-precedence.html).  
2. If you are using [Theme Style Sheet](https://docs.unity3d.com/Manual/UIE-tss.html), note that style sheets lower in the list are applied over those above.

### Create your element
We actually need to see an element to see how to style it. Create an element using the UI Builder or via code.  

Elements should generally not be styled directly via C#'s inline styles. Inline styles override all other styling, require recompilation to adjust, and must be specified every time an element is created.

#### Use BEM when choosing class names
[Block Element Modifier](https://getbem.com) (BEM) is a naming convention that is also used by UI Toolkit[^1]. You can remain consistent with the built-in controls, and keep consistency with others by using it too.

::::note  
#### Example
For our styling example we will be styling a Slider. The example will use one from the UI Toolkit Samples **Window | UI Toolkit | Samples**.

![UI Toolkit Samples - Slider](samples-slider.png){.padded}

::::

### Use the UI Toolkit Debugger to inspect your element

Use the [UI Toolkit Debugger](https://docs.unity3d.com/Manual/UIE-ui-debugger.html) to inspect the styles, types, names, classes, and hierarchy of your element. If you've used browser dev tools this should be familiar to you.

You can find the debugger at **Window | UI Toolkit | Debugger** or **Window | Analysis | UIElements Debugger** depending on Unity version, right-click on an inspector tab and select it, or press <kbd>Ctrl+F5</kbd>.

#### Inspecting your element
Select the correct window from the top left of the debugger, and select **Pick Element**. Hover your element until the portion you wish to work with is highlighted, and select it.  

^^^
<video width="750" height="325" loop muted controls><source type="video/webm" src="/HTML/ui/ui-toolkit/ui-toolkit-debugger-picking.webm"></video>
^^^ Picking the background sliding bar of the Slider.

#### The hierarchy
![UI Toolkit Debugger - Hierarchy](ui-toolkit-debugger-hierarchy.png){.padded}

Now the element has been selected (or something close to it), you can see a hierarchy of all the elements it's made of, their names, and their classes. The text in the hierarchy follows the [selector rules](#learn-selector-rules) we learned earlier.

You can hover over the elements and they will highlight in the window you are inspecting.

#### The inspector
![UI Toolkit Debugger - Inspector](ui-toolkit-debugger-inspector.png){.padded}

Here you can see the layout, stylesheets and the order they are applied, matched selectors and their precedence, state, applied classes, styles and how they are matched, and a dump of the UXML.


#### Preview style adjustments of your elements
In the **Styles** foldout you can override any style temporarily for the element (reload the window or reset its content to reset it).

^^^  
![UI Toolkit Debugger - Styles](ui-toolkit-debugger-styles.png)  
^^^ Directly adjusting the height of the example Slider.

Adjust the styles of your element, and surrounding elements under the control until you are happy with the outcome.

:::warning
These adjustments are temporary, note down what adjustments you have made, or perform the next step in parallel.
:::

### Construct a USS rule that targets your element
In your USS, reconstruct the work you have done in the debugger.  
Often, you should anchor the styling to the root of the control, using the type, its name, or class. Take note of the preexisting **Matching Selectors** already present on your element for inspiration.

^^^  
![UI Toolkit Debugger - Matching selectors](ui-toolkit-debugger-matching-selector.png)  
^^^ A preexisting selector that has been used by UI Toolkit for our element.

::::note
#### Example
^^^  
```css
.unity-base-slider__drag-container {
  /* Make the parent "drag container" align its "dragger" child in the center of the cross-axis */
  justify-content: center;
}

.unity-base-slider--horizontal .unity-base-slider__tracker {
  /* The "tracker" background can stretch across the slider (left 0 to right 0), 
     and not contribute to layout by using absolute. */
  position: absolute;
  height: 10px;
  left: 0;
  right: 0;
}

.unity-base-slider--horizontal .unity-base-slider__dragger {
  /* Make the "dragger" taller and remove previous alignment now it's centered */
  margin-top: 0;
  width: 16px;
}

.unity-base-slider--horizontal .unity-base-slider__dragger-border {
  /* Adjust the "dragger-border" to accomodate the new size of the "dragger". It's already absolute. */
  height: 14px;
  width: 20px;
  margin-left: -2px;
}
```
^^^ A descendent selector matching elements with the `unity-base-slider__tracker` class below elements with the `unity-base-slider--horizontal` class.
::::


#### Styling notes
- Make careful adjustments to the combination of selectors you use to not override the styles of other elements that might use the classes.  
  If you see unwanted elements changing due to your style sheet, you will have to construct a more specific selector.
- I find the complicated part about USS is not the selectors, but it is the layout, and ramifications layout in the hierarchy.  
  It can be tough to form an intuition about what styles across which elements will make the change you want to see. It just takes time.
- Try to reduce the amount of overrides you have. The simpler you can achieve an outcome, the easier it will be to modify it and not make a mess in the process.  
  Understand the primary and secondary axes (main and cross), and what flex and justification rules apply to them, and don't overdo it.


### Take a peek at the C# source code
It's generally good practice to take a look at the [Unity C# reference source code](https://github.com/Unity-Technologies/UnityCsReference/) to see how something works. I keep a copy downloaded to my computer so I can browse it as I work.

In my example, I've been working with a Slider, searching for it in the `UIElements` namespace you can find [the file](https://github.com/Unity-Technologies/UnityCsReference/blob/67d5d85abbea076e469a1642e04f3ab50a326bea/Modules/UIElements/Core/Controls/Slider.cs) **Modules | UIElements | Core | Controls | Slider.cs**.  

Taking a look at how it's constructed, it uses a base class to do most of the work, but we can see it adds a class to the element, and uses properties to create some nested elements, adding classes to them too.  
^^^  
```csharp
/// <summary>
/// Creates a new instance of a Slider.
/// </summary>
/// <param name="label">The string representing the label that will appear beside the field.</param>
/// <param name="start">The minimum value that the slider encodes.</param>
/// <param name="end">The maximum value that the slider encodes.</param>
/// <param name="direction">The direction of the slider (Horizontal or Vertical).</param>
/// <param name="pageSize">A generic page size used to change the value when clicking in the slider.</param>
public Slider(string label, float start = 0, float end = kDefaultHighValue, SliderDirection direction = SliderDirection.Horizontal, float pageSize = kDefaultPageSize)
    : base(label, start, end, direction, pageSize)
{
    AddToClassList(ussClassName);
    labelElement.AddToClassList(labelUssClassName);
    visualInput.AddToClassList(inputUssClassName);
}
```
^^^ One of the constructors for a Slider

These classes are `public` and `static`, so if you're ever needing them in C# you can access them directly from the class!

^^^
```csharp
/// <summary>
/// USS class name of elements of this type.
/// </summary>
public new static readonly string ussClassName = "unity-slider";
/// <summary>
/// USS class name of labels in elements of this type.
/// </summary>
public new static readonly string labelUssClassName = ussClassName + "__label";
/// <summary>
/// USS class name of input elements in elements of this type.
/// </summary>
public new static readonly string inputUssClassName = ussClassName + "__input";
```
^^^ The classes applied to Sliders as they are specified in C#.

You can investigate further, look at the base class `BaseSlider`, follow along to find out what elements it creates and what classes it adds. As most of UI Toolkit is in C#, and doesn't rely on UnityEngine.Object, most of the work is done in constructors and should be very intuitive!  

Sometimes you might find the code is applying a style inline. These styles cannot be overridden by USS. You'll be able to see this indicated in the debugger, but know you can poke around the code too!

[^1]: [Best practices for USS.](https://docs.unity3d.com/Manual/UIE-USS-WritingStyleSheets.html)