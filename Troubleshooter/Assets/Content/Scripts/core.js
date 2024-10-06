function whenReady(callback) {
    // in case the document is already rendered
    if (document.readyState !== 'loading') callback();
    // modern browsers
    else if (document.addEventListener) document.addEventListener('DOMContentLoaded', callback);
    // IE <= 8
    else document.attachEvent('onreadystatechange', function () {
            if (document.readyState === 'complete') callback();
        });
}

function indexOfChild(child) {
    const parent = child.parentNode;
    return Array.prototype.indexOf.call(parent.children, child);
}

function load(
    // Elements
    element,
    sidebarElement,
    // Ids
    contentsId,
    sidebarContentsId,
    // Request and callbacks
    content,
    callback,
    error
) {
    if (error === undefined) {
        error = e => console.log(e);
    }
    fetch(content, {credentials: "same-origin"})
        .then((response) => {
            if (response.ok)
                return response.text();
            return Promise.reject(response.status);
        })
        .then(html => {
            const nodes = new DOMParser().parseFromString(html, 'text/html');

            // Load contents
            if (element) {
                document.title = nodes.title ?? "Unity, huh, how?";

                const contents = nodes.querySelector(contentsId);
                element.replaceChildren(...contents.childNodes);

                // Recreate all script tags so they actually load.
                element.querySelectorAll("script").forEach(scriptElement => {
                    const parent = scriptElement.parentNode;
                    const functionalScript = document.createElement("script");
                    functionalScript.src = scriptElement.src;
                    functionalScript.type = scriptElement.type;
                    parent.replaceChild(functionalScript, scriptElement);
                });
            }

            // Load sidebar contents
            if (sidebarElement) {
                const sidebarContents = nodes.querySelector(sidebarContentsId);
                sidebarElement.replaceChildren(...sidebarContents.childNodes);
            }

            return html;
        })
        .then(callback)
        .catch(error);
}
