const pageParameterKey = 'page';
const contentsId = '#contents';
const sidebarContentsId = '#sidebar-contents';
const containerId = '#container';
const root = 'index';
let currentDirectory = "";

// ------ WOW ------
function resize() {
    document.documentElement.style.setProperty('--vh', `${window.innerHeight}px`);
}

window.addEventListener('resize', resize);
window.addEventListener('load', resize);

whenReady(() => {
    // Scroll redirect.
    const container = document.querySelector(containerId);
    const contents = document.querySelector(contentsId);
    container?.addEventListener('wheel', function (e) {
        if (e.target !== container) return;
        contents?.scrollBy({
            left: e.deltaX,
            top: e.deltaY,
            // behavior: 'smooth'
        });
        e.stopPropagation();
    });

    // Developer tools.
    if (window.location.host.startsWith("localhost:")) {
        document.getElementById("local-developer-tools").classList.remove("hidden");
    }
});

// Declare early.
function postText(key, value) {
    fetch(`${window.location.origin}/${key}/${value}`, {
        method: 'POST',
        headers: {
            'Accept': 'text/plain',
            'Content-Type': 'text/plain'
        }
    }).then(response => {
        console.log(response);
        if (response.status === 200) {
            location.reload();
        }
    });
}


// -----------------

class CodeSettings {
    usesLigatures;
    theme;
    storage;

    static get LigaturesKey() {
        return "help_CodeLigatures"
    };

    static get ThemeKey() {
        return "help_CodeTheme"
    };

    constructor(storage) {
        this.storage = storage;
        const ligaturesValue = storage.getItem(CodeSettings.LigaturesKey);
        const themeValue = storage.getItem(CodeSettings.ThemeKey);

        this.usesLigatures = ligaturesValue == null ? true : ligaturesValue === "true";
        this.theme = themeValue == null ? "rider-dark" : themeValue;
    }

    UpdateLigatures() {
        storage.setItem(CodeSettings.LigaturesKey, this.usesLigatures);
        if (this.usesLigatures) {
            removeCssRuleIfRequired(CodeSettings.LigaturesKey)
        } else {
            addCssRuleIfRequired("pre { font-variant-ligatures: none; }", CodeSettings.LigaturesKey)
        }
    }

    UpdateTheme() {
        // If theme is not valid, set it to default.
        if (this.theme !== 'rider-dark' &&
            this.theme !== 'one-monokai' &&
            this.theme !== 'dracula' &&
            this.theme !== 'vs-dark') {
            this.theme = 'rider-dark';
        }
        storage.setItem(CodeSettings.ThemeKey, this.theme);
        document.documentElement.className = this.theme;
    }
}

const storage = window.localStorage;
let codeSettings = new CodeSettings(storage);

let pageParam = getPageParameter();
// Don't push a history state for this change.
if (pageParam === root) {
    // If we've landed on the index then we should set our location as such.
    setPage('', '', getHash(), false);
    pageParam = null;
} else {
    setPage(pageParam, pageParam, getHash(), false);
}

loadPageWithoutLink(pageParam, getHash());


/*************** CALLBACKS ****************/

// Popstate is for capturing forward-back events from history.pushState
window.addEventListener("popstate", loadPageFromState);

function loadPageFromState(e) {
    // page reload
    if (e.state) {
        loadPageFromLink(e.state.pathParameter, e.state.hashParameter, {
            setParameter: false,
            useCurrentDirectory: false
        });
    } else {
        loadPageFromLink(getPageParameter(), getHash(), {setParameter: false, useCurrentDirectory: false});
    }
}

/*************** FUNCTIONS *******************/

function getPageParameter() {
    return getParameterByKey(pageParameterKey);
}

function getParameterByKey(key, url) {
    if (!url) url = window.location.href;
    key = key.replace(/[\[\]]/g, '\\$&');
    const regex = new RegExp('[?&]' + key + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return results[2];
}

function getHash() {
    return window.location.hash;
}

function setPage(value, url, hash, pushHistory = true) {
    url = url == null || url === "" ? window.location.pathname.replace(/^\/\//, "") : url;

    if (hash !== '')
        url += hash;
    // State Object, Page Name, URL
    if (pushHistory)
        window.history.pushState({pathParameter: value, hashParameter: hash}, "", url);
    else
        window.history.replaceState({pathParameter: value, hashParameter: hash}, "", url);
}

// Load Page is called from HTML
// noinspection JSUnusedGlobalSymbols
function loadPage(link, replaceState = false) {
    if (link == null || link === "") {
        console.error('Ignored page load as button links to empty location');
        return;
    }

    if (link === root)
        link = null;

    let hash = '';
    if (link != null) {
        const hashIndex = link.indexOf("#");
        if (hashIndex >= 0) {
            hash = link.substring(hashIndex);
            link = link.substring(0, hashIndex);
        }
    }

    loadPageFromLink(link, hash, {replaceState: replaceState});
}

function loadPageNonRelative(absoluteLink) {
    if (absoluteLink == null || absoluteLink === "") {
        console.error('Ignored page load as button links to empty location');
        return;
    }
    if (absoluteLink === root)
        absoluteLink = null;
    loadPageFromLink(absoluteLink, '', {useCurrentDirectory: false});
}

// Load Hash is called from HTML
// noinspection JSUnusedGlobalSymbols
function loadHash(hash) {
    // Scroll to the hash and copy the page to the clipboard.
    const pageParameter = getPageParameter();
    setPage(pageParameter, pageParameter, hash);
    scrollToHash(hash);
    copyTextToClipboard(window.location.href);
}

function processPageValue(value) {
    if (value === null)
        value = location.pathname.slice(1);
    if (value == null || value === "" || value === "index.html")
        return root;
    value = value.replace(/\.[^/.]+$/, "");
    value = value.replace('%20', "-");
    return value.toLowerCase();
}

function fireCallbackIfPageIsCurrent(callback) {
    const pageParameter = processPageValue(null);
    window.addEventListener("loadedFromState", event => {
        if (event !== undefined && event.detail !== pageParameter)
            return;
        callback();
    });
}

function loadPageFromLink(value, hash, {setParameter = true, useCurrentDirectory = true, replaceState = false}) {
    value = processPageValue(value);
    if (value.length > 0 && value[0] === '/') {
        useCurrentDirectory = false;
        value = value.substring(1);
    }

    let valueToLoad = value;
    if (useCurrentDirectory && currentDirectory !== "")
        valueToLoad = absolute(`${currentDirectory}/`, value)
    let url = value;

    whenReady(function () {
        const contents = document.querySelector(contentsId);
        const sidebarContents = document.querySelector(sidebarContentsId);
        try {
            // Load the page
            load(contents, sidebarContents, contentsId, sidebarContentsId, `/${valueToLoad}.html`, () => {
                performPageSetup(valueToLoad, url, hash, {
                    setParameter: setParameter,
                    useCurrentDirectory: useCurrentDirectory,
                    replaceState: replaceState
                })
            }, load404);
            document.getElementById('page-search').value = "";
        } catch {
            load404();
        }
    });
}

function loadPageWithoutLink(value, hash) {
    let useCurrentDirectory = true;
    value = processPageValue(value);
    if (value.length > 0 && value[0] === '/') {
        useCurrentDirectory = false;
        value = value.substring(1);
    }

    whenReady(function () {
        let valueToLoad = value;
        if (useCurrentDirectory && currentDirectory !== "")
            valueToLoad = absolute(`${currentDirectory}/`, value)
        performPageSetup(valueToLoad, value, hash, {
            setParameter: false,
            useCurrentDirectory: useCurrentDirectory,
            replaceState: false,
            scriptReload: false
        })
    });
}

function performPageSetup(valueToLoad, url, hash, {
    setParameter = true,
    useCurrentDirectory = true,
    replaceState = false,
    scriptReload = true
}) {
    currentDirectory = valueToLoad.replace(/\/*[^/]+$/, "");
    if (setParameter) {
        if (!useCurrentDirectory && url.length > 0 && url[0] !== '/')
            url = `/${url}`;
        setPage(valueToLoad, url, hash, !replaceState);
    }

    // Anything that can affect layout
    setupCodeSettings();
    if (scriptReload)
        reloadScripts(valueToLoad);
    // -------------------------------
    setTimeout(() => scrollToHash(hash), 100); // Delay seems to be required in some cases.
    setupHeaders();
}

function absolute(base, relative) {
    const stack = base.split("/"),
        parts = relative.split("/");
    stack.pop(); // remove current file name (or empty string)
                 // (omit if "base" is the current folder without trailing slash)
    for (let i = 0; i < parts.length; i++) {
        if (parts[i] === ".")
            continue;
        if (parts[i] === "..")
            stack.pop();
        else
            stack.push(parts[i]);
    }
    return stack.join("/");
}

function load404() {
    loadPageFromLink(`404`, '', {setParameter: false, useCurrentDirectory: false});
}

function scrollToHash(hash) {
    if (hash === '') {
        document.querySelector(contentsId).scrollTo(0, 0);
        return;
    }
    const hashElement = document.getElementById(hash.substring(1));
    if (hashElement == null) return;

    resize(); // Call to forcibly set --vh
    const container = document.querySelector(containerId);
    const contents = document.querySelector(contentsId);

    // The padding needs to equal the height of the container minus the distance to the bottom of the hash element.
    let computedStyle = window.getComputedStyle(contents);
    const containerHeight = container.clientHeight - parseFloat(computedStyle.paddingTop);
    const distToElementBottom = getDistanceFromTopToBottom(hashElement, contents.lastElementChild);
    if (distToElementBottom < containerHeight) {
        let padding = containerHeight - distToElementBottom;
        document.body.style.setProperty("--bottom-padding", `${padding}px`);
    } else {
        document.body.style.removeProperty("--bottom-padding",);
    }

    hashElement.scrollIntoView();
}

function getDistanceFromTopToBottom(from, to) {
    const fromRect = from.getBoundingClientRect();
    const toRect = to.getBoundingClientRect();
    return toRect.bottom - fromRect.top;
}

function setupHeaders() {
    document.querySelector(contentsId).querySelectorAll("h1, h2, h3, h4, h5, h6").forEach(
        e => {
            e.addEventListener("mouseenter", () => e.querySelector(".header-permalink")?.classList.add("show"));
            e.addEventListener("mouseleave", () => e.querySelector(".header-permalink")?.classList.remove("show"));
        }
    );
    let sidebar = document.querySelector(sidebarContentsId);
    sidebar?.querySelectorAll(".header-permalink").forEach(e => e.remove());
    sidebar?.querySelectorAll("h1, h2, h3, h4, h5, h6").forEach(e => e.removeAttribute("id"));
}

function setupCodeSettings() {
    // Ligatures setting
    codeSettings.UpdateLigatures();
    document.querySelectorAll(".code-setting-ligatures").forEach(
        e => {
            e.innerHTML = codeSettings.usesLigatures ? "Ligatures ✓" : "Ligatures";
            e.addEventListener("click", function () {
                codeSettings.usesLigatures = !codeSettings.usesLigatures;
                codeSettings.UpdateLigatures();
                e.innerHTML = codeSettings.usesLigatures ? "Ligatures ✓" : "Ligatures";
            });
        });
    // Theme setting
    codeSettings.UpdateTheme();
    document.querySelectorAll(".code-setting-theme").forEach(
        e => {
            e.addEventListener("click",
                function () {
                    if (codeSettings.theme === "rider-dark")
                        codeSettings.theme = "vs-dark";
                    else if (codeSettings.theme === "vs-dark")
                        codeSettings.theme = "one-monokai";
                    else if (codeSettings.theme === "one-monokai")
                        codeSettings.theme = "dracula";
                    else
                        codeSettings.theme = "rider-dark";
                    codeSettings.UpdateTheme();
                }
            );
        });
    document.querySelectorAll(".code-setting-copy").forEach(
        e => {
            e.addEventListener("click",
                function () {
                    const container = e.closest('div[class^="code-container"]');
                    const inner = container.querySelector('.code-container-inner');
                    const r = document.createRange();
                    r.selectNode(inner);
                    if (!navigator.clipboard) {
                        copyFallback(getRangeSelection(r));
                    } else {
                        const selection = getRangeSelection(r);
                        navigator.clipboard.writeText(selection.toString())
                            .then(() => selection.removeAllRanges())
                            .catch(() => copyFallback(r));
                    }
                }
            );
        });
}

function getRangeSelection(range) {
    const selection = window.getSelection();
    selection.removeAllRanges();
    selection.addRange(range);
    return selection;
}

function copyFallback(selection) {
    document.execCommand('copy');
    selection.removeAllRanges();
}

function fallbackCopyTextToClipboard(text) {
    const textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        document.execCommand('copy');
    } finally {
        document.body.removeChild(textArea);
    }
}

function copyTextToClipboard(text) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text);
        return;
    }
    navigator.clipboard.writeText(text).catch();
}

function reloadScripts(value) {
    const event = new CustomEvent('loadedFromState', {
        detail: value,
        bubbles: false,
        cancelable: true,
        composed: false
    })
    window.dispatchEvent(event);
}

function removeCssRuleIfRequired(id) {
    const element = document.getElementById(id)
    if (element == null)
        return;
    const head = document.getElementsByTagName("head")[0];
    head.removeChild(element);
}

function addCssRuleIfRequired(rule, id) {
    if (document.getElementById(id) != null)
        return;
    const head = document.getElementsByTagName("head")[0];

    const style = document.createElement('style');

    style.innerHTML = rule;
    style.id = id;

    head.appendChild(style);
}

function reportIssue() {
    let page = processPageValue(null);
    fetch("/Json/source-index.json")
        .then(response => response.json())
        .then(json => {
            if (page === null) page = root;
            let source = json.pageToSourcePath[page];
            let currentLoc = encodeURIComponent(window.location.href);
            if (source === 'undefined') {
                window.location.href = `https://github.com/vertxxyz/help.vertx.xyz/issues/new?title=&body=%0A%0A%5BEnter%20feedback%20here%5D%0A%0A%0A---%0A%23%23%23%23%20Document%20Details%0A%0A%3C!--%20%E2%9A%A0%20*Do%20not%20edit%20this%20section.*%20--%3E%0A%0A*%20Content%3A%20%5B${page}%5D(${currentLoc})%0A*%20Content%20Source%3A%20Unknown&labels=content`;
            } else {
                source = encodeURIComponent(source);
                window.location.href = `https://github.com/vertxxyz/help.vertx.xyz/issues/new?title=&body=%0A%0A%5BEnter%20feedback%20here%5D%0A%0A%0A---%0A%23%23%23%23%20Document%20Details%0A%0A%3C!--%20%E2%9A%A0%20*Do%20not%20edit%20this%20section.*%20--%3E%0A%0A*%20Content%3A%20%5B${page}%5D(${currentLoc})%0A*%20Content%20Source%3A%20%5B${source}%5D(https://github.com/vertxxyz/help.vertx.xyz/tree/main/Troubleshooter${source})&labels=content`;
            }
        });
}

function toggleCollapsedCode(button) {
    const collapsed = "collapsable--collapsed";
    const expanded = "collapsable--expanded";
    let collapsable = button.closest(".collapsable");
    if (collapsable.classList.contains(collapsed)) {
        collapsable.classList.remove(collapsed);
        collapsable.classList.add(expanded);
    } else {
        collapsable.classList.remove(expanded);
        collapsable.classList.add(collapsed);
    }
}
