const pageParameterKey = 'page';
const main = 'Main';
let currentDirectory = "";

let pageParam = getPageParameter();
console.log(window.location.href);
console.log(`Initial Page Param: ${pageParam}`);
//Don't push a history state for this change.
if (pageParam === main) {
	//If we've landed on the index then we should set our location as "Main".
	setParameterByKey(pageParameterKey, '', getHash(), false);
	pageParam = null;
} else {
	setParameterByKey(pageParameterKey, pageParam, getHash(), false);
}

loadPageFromLink(pageParam, getHash(), false);


/*************** CALLBACKS ****************/

// Popstate is for capturing forward-back events from history.pushState
window.onpopstate = loadPageFromState;

function loadPageFromState(e) {
	// page reload
	if (e.state) {
		console.log(`PopState: ${e.state.pathParameter} - ${e.state.hashParameter}`);
		loadPageFromLink(e.state.pathParameter, e.state.hashParameter, false, false);
	} else {
		loadPageFromLink(getPageParameter(), getHash(), false, false);
	}
}

/*************** FUNCTIONS *******************/

function getPageParameter () { return getParameterByKey(pageParameterKey); }

function getParameterByKey(key, url) {
	if (!url) url = window.location.href;
	key = key.replace(/[\[\]]/g, '\\$&');
	const regex = new RegExp('[?&]' + key + '(=([^&#]*)|&|#|$)'),
		results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return results[2];
}

function getHash () { return window.location.hash; }

function setParameterByKey(key, value, hash, pushHistory = true) {
	let url = value == null || value === "" ? window.location.pathname : `?${key}=${value}`;

	if(hash !== '')
		url += hash;
	console.log(`setParameterByName: ${url} - ${hash}`);
	// State Object, Page Name, URL
	if (pushHistory)
		window.history.pushState({pathParameter: value, hashParameter: hash}, document.title, url);
	else
		window.history.replaceState({pathParameter: value, hashParameter: hash}, document.title, url);
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
	loadPageFromLink(relativeLink, '');
}

function loadPageFromLink(value, hash, setParameter = true, useCurrentDirectory = true) {

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
		const sidebarContents = $('.sidebar-contents');
		try {
			contents.load(`HTML/${value}.html`, function (response, status, xhr) {
				if( status === "error") {
					load404();
					return;
				}
				if (setParameter)
					setParameterByKey(pageParameterKey, value, hash);
				Prism.highlightAll();

				if(hash !== ''){
					/*contents.animate({
				        scrollTop: $(hash).offset().top
				    }, 400);*/
					$(hash).get(0).scrollIntoView()
				}
			});
			sidebarContents.load(`HTML/${value}_sidebar.html`, function (response, status, xhr) {
				if( status === "error")
					sidebarContents.empty();
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
	loadPageFromLink(`404`, '', false, false);
}