function highlightTable(element) {
    addOrRemoveFromElement(element, true);
}

function unhighlightTable(element) {
    addOrRemoveFromElement(element, false);
}

function addOrRemoveFromElement(element, add) {
    const el = $(element);
    const index = el.index();
    const parent = el.parent();
    const row = parent.children().first();
    const column = parent.closest('table').find('th').eq(index);
    if(add) {
        el.addClass('highlight');
        row.addClass('highlight');
        column.addClass('highlight');
    } else {
        el.removeClass('highlight');
        row.removeClass('highlight');
        column.removeClass('highlight');
    }
}