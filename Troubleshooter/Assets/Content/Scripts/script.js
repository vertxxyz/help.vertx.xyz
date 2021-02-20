const pageParameterKey = 'page';
const main = 'Main';
let currentDirectory = "";

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

loadPageFromLink(pageParam, false);


/*************** CALLBACKS ****************/

// Popstate is for capturing forward-back events from history.pushState
window.onpopstate = loadPageFromState;

function loadPageFromState(e) {
	// page reload
	if (e.state) {
		console.log(`PopState: ${e.state.pathParameter}`);
		loadPageFromLink(e.state.pathParameter, false, false);
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
	return results[2];
}

function setParameterByKey(key, value, pushHistory = true) {
	const url = value == null || value === "" ? window.location.pathname : `?${key}=${value}`;

	console.log(`setParameterByName: ${url}`);
	// State Object, Page Name, URL
	if (pushHistory)
		window.history.pushState({pathParameter: value}, document.title, url);
	else
		window.history.replaceState({pathParameter: value}, document.title, url);
}

//Load Page is called from HTML
function loadPage(relativeLink) {
	if (relativeLink == null || relativeLink === "") {
		console.log('Ignored page load as button links to empty location');
		return;
	}
	console.log(`Load Page: ${relativeLink}`);
	if (relativeLink === main)
		relativeLink = null;
	loadPageFromLink(relativeLink);
}

function loadPageFromLink(value, setParameter = true, useCurrentDirectory = true) {

	if (value == null || value === "")
		value = main;
	else
		value = value.replace(/\.[^/.]+$/, "");

	if (useCurrentDirectory && currentDirectory !== "")
		value = absolute(`${currentDirectory}/`, value)
	else
		value = `${value}`;

	currentDirectory = value.replace(/\/*[^/]+$/, "");

	console.log(`Load Page Contents: \"${value}\" - current directory: \"${currentDirectory}\"`);

	$(document).ready(function () {
		const contents = $('.contents');
		try {
			contents.load(`HTML/${value}.html`, function (response, status, xhr) {
			    if( status === "error") {
			        load404();
			        return;
                }
				if (setParameter)
					setParameterByKey(pageParameterKey, value);
				Prism.highlightAll();
			});
		} catch {
			load404();
		}
	});
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
	//console.log('loading 404')
	loadPageFromLink(`404`, false, false);
}