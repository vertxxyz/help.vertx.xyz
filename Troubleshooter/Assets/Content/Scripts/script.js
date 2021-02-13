const pageParameterKey = 'page';
const main = 'Main';

let pageParam = getParameterByKey(pageParameterKey);
console.log(`Initial Page Param: ${pageParam}`);
//Don't push a history state for this change.
if (pageParam === main) {
    //If we've landed on the index then we should set our location as "Main".
    setParameterByKey(pageParameterKey, '', false);
    pageParam = null;
} else {
    setParameterByKey(pageParameterKey, pageParam, false);
}

let linksToGuids;
let guidsToLinks;
$.getJSON('JSON/links.json', function (data) {
    linksToGuids = data['LinksToGuids'];
    guidsToLinks = data['GuidsToLinks'];

    //Load whatever page is the first page we've landed on.
    loadPageFromLink(pageParam);
});


/*************** CALLBACKS ****************/

// Popstate is for capturing forward-back events from history.pushState
window.onpopstate = loadPageFromState;

function loadPageFromState(e) {
    // page reload
    if (e.state) {
        console.log(`PopState: ${e.state.pathParameter}`);
        loadPageFromLink(e.state.pathParameter);
    } else {

    }
}

/*************** FUNCTIONS *******************/

function getParameterByKey(key, url) {
    if (!url) url = window.location.href;
    key = key.replace(/[\[\]]/g, '\\$&');
    const regex = new RegExp('[?&]' + key + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function setParameterByKey(key, value, pushHistory = true) {
    const url = value == null ? window.location.pathname : `?${key}=${value}`;

    console.log(`setParameterByName: ${url}`);
    // State Object, Page Name, URL
    if (pushHistory)
        window.history.pushState({pathParameter: value}, document.title, url);
    else
        window.history.replaceState({pathParameter: value}, document.title, url);
}

//Load Page is called from HTML
function loadPage(guid) {
    if (guid == null || guid === "") {
        console.log('Ignored page load as button links to empty location');
        return;
    }
    let value = guidsToLinks[guid];
    console.log(`Load Page: ${value}`);
    if (value === main)
        value = null;
    setParameterByKey(pageParameterKey, value);
    loadPageFromLink(value);
}

function loadPageFromLink(value) {
    if (value == null)
        value = main;

    console.log(`Load Page Contents: \"${value}\"`);

    let guid = linksToGuids[value];
    loadPageFromGuid(guid);
}

function load404() {
    console.log('loading 404')
    loadPage(`404`);
}

function loadPageFromGuid(guid) {
    if (guid === undefined) {
        load404();
        return;
    }
    $(document).ready(function () {
        const contents = $('.contents');
        try {
            contents.load(`HTML/${guid}.html`, function () {
                Prism.highlightAll();
            });
        } catch {
            load404();
        }
    });
}