let defaultValue, currentTooltip = null, currentTarget = null;
let lastFill = null, lastStroke = null, lastTextColor = null;
let tooltipContainer = null;

var reload = () => {
    tooltipContainer = document.querySelector("#ref-tooltip");
    defaultValue = tooltipContainer?.textContent;

    const tooltipDataContainer = document.querySelector("#ref-tooltip-data");
    const tooltipLookup = {};
    if (tooltipDataContainer != null) {
        for (let child of tooltipDataContainer.children) {
            if (child.dataset.title === undefined || child.dataset.keys === undefined) continue;
            let lookup = tooltipLookup[child.dataset.title];
            if (lookup === undefined)
                lookup = tooltipLookup[child.dataset.title] = {};
            const keys = child.dataset.keys.split(', ');
            for (let key of keys)
                lookup[key] = child;
        }
    }

    document.querySelectorAll("svg").forEach(svg => {
        let title = svg.querySelector(":scope > title");
        const lookup = tooltipLookup[title?.innerHTML];
        if (lookup === undefined) return;
        title.innerHTML = '';
        svg.onmouseover = e => handleTooltip(e, lookup);
        svg.onclick = e => handleTooltip(e, lookup);
        svg.onmouseleave = () => showDefaultTooltip();
    });
};

fireCallbackIfPageIsCurrent(reload);
reload();

function handleTooltip(e, lookup) {
    const result = lookup[e.target.dataset?.name];
    if (result === undefined) return;
    if (tooltipContainer == null) return;

    // Go up the hierarchy to the root of the data element
    let target = e.target;
    while (target.parentElement.dataset.name === e.target.dataset.name) {
        target = target.parentElement;
    }

    if (currentTarget === target) return;

    hoverNewTarget(target);

    if (currentTooltip === result) return;

    tooltipContainer.replaceChildren(...result.cloneNode(true).childNodes);
    
    // Set the columns to be equal width.
    tooltipContainer.querySelectorAll("colgroup").forEach(cg => {
       const val = 100.0 / cg.childElementCount - 0.001;
       cg.querySelectorAll(":scope > col").forEach(col => {
           col.setAttribute("style", `width:${val}%`); 
       })
    });
    currentTooltip = result;
}

function showDefaultTooltip() {
    tooltipContainer.innerHTML = `<div class="center"><p>${defaultValue}</p></div>`;
    currentTooltip = null;
    hoverNewTarget(null);
}

function hoverNewTarget(target) {
    if(target === currentTarget) return;
    if (currentTarget != null) {
        setTargetFillAndStroke(lastFill, lastStroke);
        setTargetTextColor(lastTextColor);
    }
    currentTarget = target;
    if(target == null) return;
    setTargetFillAndStroke("#8f8", "#000");
    setTargetTextColor("#000");
}

function setTargetAttributeAndReturnPrev(attr, value) {
    if (currentTarget == null) return;
    for (let child of currentTarget.children) {
        if (!child.hasAttribute(attr)) continue;
        const prev = child.getAttribute(attr);
        child.setAttribute(attr, value);
        return prev;
    }
    return null;
}

function setTargetFill(color) { lastFill = setTargetAttributeAndReturnPrev("fill", color); }
function setTargetStroke(color) { lastStroke = setTargetAttributeAndReturnPrev("stroke", color); }

function setTargetFillAndStroke(fill, stroke) {
    setTargetFill(fill);
    setTargetStroke(stroke);
}

function setTargetTextColor(color) {
    if (currentTarget == null) return;
    const text= currentTarget.querySelector("text")
    if (text == null) return;
    if(!text.parentElement.hasAttribute("fill")) return;
    lastTextColor = text.parentElement.getAttribute("fill");
    text.parentElement.setAttribute("fill", color);
}