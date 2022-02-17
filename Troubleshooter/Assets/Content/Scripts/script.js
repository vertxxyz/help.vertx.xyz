const pageParameterKey = 'page';
const contentsClass = '.contents';
const main = 'Main';
let currentDirectory = "";

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
//Don't push a history state for this change.
if (pageParam === main) {
	//If we've landed on the index then we should set our location as "Main".
	setPage('', '', getHash(), false);
	pageParam = null;
} else {
	setPage(pageParam, pageParam, getHash(), false);
}

loadPageFromLink(pageParam, getHash(), false);


/*************** CALLBACKS ****************/

// Popstate is for capturing forward-back events from history.pushState
window.onpopstate = loadPageFromState;

function loadPageFromState(e) {
	// page reload
	if (e.state) {
		loadPageFromLink(e.state.pathParameter, e.state.hashParameter, false, false);
	} else {
		loadPageFromLink(getPageParameter(), getHash(), false, false);
	}
	// TODO reload script elements.
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
	url = url == null || url === "" ? window.location.pathname : url;

	if (hash !== '')
		url += hash;
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
	if (relativeLink === main)
		relativeLink = null;
	loadPageFromLink(relativeLink, '', true, true);
}

//Load Hash is called from HTML
function loadHash(hash) {
	// Scroll to the hash and copy the page to the clipboard.
	const pageParameter = getPageParameter();
	setPage(pageParameter, pageParameter, hash);
	scrollToHash(hash);
	copyTextToClipboard(window.location.href);
}

function processPageValue(value) {
	if(value === null)
		value = location.pathname.slice(1);
	if (value == null || value === "")
		value = main;
	else {
		value = value.replace(/\.[^/.]+$/, "");
		value = value.replace('%20', "-");
		value = value.toLowerCase();
	}
	return value;
}

function loadPageFromLink(value, hash, setParameter = true, useCurrentDirectory = true) {
	value = processPageValue(value);
	let valueToLoad = value;
	if (useCurrentDirectory && currentDirectory !== "")
		valueToLoad = absolute(`${currentDirectory}/`, value)
	
	currentDirectory = valueToLoad.replace(/\/*[^/]+$/, "");

	$(document).ready(function () {
		const contents = $(contentsClass);
		try {
			// Load the page
			contents.load(`/HTML/${valueToLoad}.html`, function (response, status, xhr) {
				if (status === "error") {
					load404();
					return;
				}
				if (setParameter)
					setPage(valueToLoad, value, hash);

				scrollToHash(hash);
				setupHeaders();
				Prism.highlightAll();
				setupCodeSettings();
				processNomnoml();
			});
			document.getElementById('page-search').value = "";
			const sidebarContents = $('.sidebar-contents');
			sidebarContents.load(`/HTML/${valueToLoad}_sidebar.html`, function (response, status, xhr) {
				if (status === "error" || response.startsWith("<!DOCTYPE html>"))
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
	loadPageFromLink(`404`, '', false, false);
}

function scrollToHash(hash) {
	if (hash === '') {
		$(contentsClass).scrollTop(0);
		return;
	}
	const hashElement = document.getElementById(hash.substring(1));
	if (hashElement == null) return;
	hashElement.scrollIntoView();
}

function setupHeaders() {
	$("h1, h2, h3, h4, h5, h6").hover(function () {
			$(this).find(".header-permalink").addClass("show");
		}, function () {
			$(this).find(".header-permalink").removeClass("show");
		}
	);
}

function setupCodeSettings() {
	// Ligatures setting
	const ligaturesSetting = $(".code-setting-ligatures");
	codeSettings.UpdateLigatures();
	ligaturesSetting.html(codeSettings.usesLigatures ? "Ligatures ✓" : "Ligatures");
	ligaturesSetting.click(function () {
		codeSettings.usesLigatures = !codeSettings.usesLigatures;
		codeSettings.UpdateLigatures();
		ligaturesSetting.html(codeSettings.usesLigatures ? "Ligatures ✓" : "Ligatures");
	});
	// Theme setting
	codeSettings.UpdateTheme();
	$(".code-setting-theme").click(function () {
		if (codeSettings.theme === "rider-dark")
			codeSettings.theme = "vs-dark";
		else if (codeSettings.theme === "vs-dark")
			codeSettings.theme = "one-monokai";
		else if (codeSettings.theme === "one-monokai")
			codeSettings.theme = "dracula";
		else
			codeSettings.theme = "rider-dark";
		codeSettings.UpdateTheme();
	});
	$(".code-setting-copy").click(function () {
		const container = $(this).closest('div[class^="code-container"]');
		const inner = container.find('.code-container-inner').get(0);
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

function processNomnoml() {
	function htmlDecode(input) {
		const doc = new DOMParser().parseFromString(input, "text/html");
		return doc.documentElement.textContent;
	}

	const graphs = document.getElementsByClassName('nomnoml');
	for (let i = 0; i < graphs.length; i++) {
		try {
			graphs[i].innerHTML = nomnoml.renderSvg(htmlDecode(graphs[i].innerHTML));
		} catch (e) {
			(console.error || console.log).call(console, e.stack || e);
			continue;
		}
		graphs[i].classList.add("processed-nomnoml");
	}
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
	let param = getPageParameter();
	fetch("/Json/source-index.json")
		.then(response => response.json())
		.then(json => {
			if (param === null) param = "main";
			let source = json.pageToSourcePath[param];
			let currentLoc = encodeURIComponent(window.location.href);
			if (source === 'undefined') {
				window.location.href = `https://github.com/vertxxyz/help.vertx.xyz/issues/new?title=&body=%0A%0A%5BEnter%20feedback%20here%5D%0A%0A%0A---%0A%23%23%23%23%20Document%20Details%0A%0A%E2%9A%A0%20*Do%20not%20edit%20this%20section.*%0A%0A*%20Content%3A%20%5B${param}%5D(${currentLoc})%0A*%20Content%20Source%3A%20Unknown&labels=content`;
			} else {
				source = encodeURIComponent(source);
				window.location.href = `https://github.com/vertxxyz/help.vertx.xyz/issues/new?title=&body=%0A%0A%5BEnter%20feedback%20here%5D%0A%0A%0A---%0A%23%23%23%23%20Document%20Details%0A%0A%E2%9A%A0%20*Do%20not%20edit%20this%20section.*%0A%0A*%20Content%3A%20%5B${param}%5D(${currentLoc})%0A*%20Content%20Source%3A%20%5B${source}%5D(https://github.com/vertxxyz/help.vertx.xyz/tree/main/Troubleshooter/${source})&labels=content`;
			}
		});
}