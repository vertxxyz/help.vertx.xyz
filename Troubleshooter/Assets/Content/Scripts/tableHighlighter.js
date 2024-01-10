function highlightTable(element) {
    addOrRemoveFromElement(element, true);
}

function unhighlightTable(element) {
    addOrRemoveFromElement(element, false);
}

function addOrRemoveFromElement(element, add) {
    const el = element;
    const parent = el.parentNode;
    const index = indexOfChild(element);
    const row = parent.children[0];
    const column = parent.closest('table').querySelectorAll('th')[index];
    if (add) {
        el?.classList.add('highlight');
        if (column == null || row == null) return;
        row.classList.add('highlight');
        column.classList.add('highlight');
    } else {
        el?.classList.remove('highlight');
        if (column == null || row == null) return;
        row.classList.remove('highlight');
        column.classList.remove('highlight');
    }
}
