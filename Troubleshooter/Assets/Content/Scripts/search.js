var searchIndex;
var searchIsClear = true;

/* search-index.json:
* 	filePaths
* 		string[]
* 	fileHeaders
* 		string[]
* 	termsToFileIndices
* 		Dictionary<string, int>
* 	common
* 		string[]
*/
fetch("/Json/search-index.json")
	.then(response => response.json())
	.then(json => searchIndex = json);

whenReady( function () {
	const pageSearch = document.getElementById('page-search');
	pageSearch.addEventListener('input', searchChange);
});

function searchChange(e) {
	if (searchIndex == null) return;

	if (e.length < 3) {
		resetPage();
		return;
	}

	if (!performSearch(e.target.value)) {
		resetPage();
		return;
	}
	searchIsClear = false;
}

function resetPage() {
	if(searchIsClear) return;
	searchIsClear = true;
	const sidebarContents = document.querySelector('.sidebar-contents');
	if(sidebarContents.length === 0) return;
	const value = processPageValue(getPageParameter());
	load(sidebarContents, `/HTML/${value}_sidebar.html`, (response) => {
		if (response.startsWith("<!DOCTYPE html>"))
			sidebarContents.replaceChildren();
	});
}

function performSearch(text) {
	const query = text.replace(/^[\s.]+|[\s.]+$/g, '').toLowerCase();
	let terms = query.split(/[\s.]+/);
	const combined = query.replace(/\s+/g, '').toLowerCase();

	// Do not search for terms that are less than 3 characters in length.
	terms = terms.filter(v => {
		return v.length >= 3;
	});
	
	if(terms.length === 0) return;

	let results = getSearchResults(terms, query);
	const results2 = getSearchResults([combined], query);

	results = results.concat(results2);

	function unique(lst) {
		const a = lst.concat();
		for (let i = 0; i < a.length; ++i) {
			for (let j = i + 1; j < a.length; ++j) {
				if (a[i] === a[j])
					a.splice(j--, 1);
			}
		}

		return a;
	}

	results = unique(results);
	displayResults(results)
	return true;
}

function displayResults(results) {
	const sidebarContents = document.querySelector('.sidebar-contents');
	sidebarContents.replaceChildren();
	const div = document.createElement("div");
	div.classList.add('search-results');
	for (const result of results) {
		const searchResultDiv = document.createElement("div");
		searchResultDiv.classList.add('search-result');
		{
			const url = getPageURL(result);
			const title = getPageTitle(result);
			
			const link = document.createElement("a");
			link.innerHTML = title;
			link.onclick = () => loadPageFromLink(url, '', true, false);
			searchResultDiv.append(link);
			
			searchResultDiv.append(document.createElement("br"));

			const urlDiv = document.createElement("span");
			urlDiv.classList.add('search-result-url');
			urlDiv.innerText = url.slice(0, -5);
			
			searchResultDiv.append(urlDiv);
		}
		div.append(searchResultDiv);
	}
	sidebarContents.append(div);
}

function getSearchResults(terms, query) {
	let page, j, i, term;
	const common = searchIndex.common;
	const termLookup = searchIndex.termsToFileIndices;
	const score = {};
	let min_score = terms.length;
	const scoredPages = new Set();
	for (i = 0; i < terms.length; i++) {
		scoredPages.clear();
		term = terms[i];
		if (common[term])
			min_score--;

		if (termLookup[term]) {
			const termResults = termLookup[term];
			for (j = 0; j < termResults.length; j++) {
				page = termResults[j];
				if(scoredPages.has(page)) continue;
				if (!score[page])
					score[page] = 0;
				++score[page];
				scoredPages.add(page);
			}
		}

		for (const si in termLookup) {
			if (si.length <= term.length) continue;
			if (si.slice(0, term.length) !== term) continue;
			const termResults = termLookup[si];
			for (j = 0; j < termResults.length; j++) {
				page = termResults[j];
				if(scoredPages.has(page)) continue;
				if (!score[page])
					score[page] = 0;
				++score[page];
				scoredPages.add(page);
			}
		}
	}
	let results = [];
	for (page in score) {
		const title = getPageTitle(page);
		// ignore partial matches
		if (score[page] < min_score) continue;
		
		const titleLower = title.toLowerCase();
		
		results.push(page);
		let placement;
		for (i = 0; i < terms.length; i++) {
			term = terms[i];
			if ((placement = titleLower.indexOf(term)) > -1) {
				score[page] += 50;
				if (placement === 0 || title[placement - 1] === '.')
					score[page] += 500;
				if (placement + term.length === title.length || title[placement + term.length] === '.')
					score[page] += 500;
			}
		}
		if (titleLower === query)
			score[page] += 10000;
		else if ((placement = titleLower.indexOf(query)) > -1)
			score[page] += ((placement < 100) ? (200 - placement) : 100);
	}

	results = results.sort(function (a, b) {
		if (score[b] === score[a]) { // sort alphabetically by title if score is the same
			const x = getPageTitle(a).toLowerCase();
			const y = getPageTitle(b).toLowerCase();
			return ((x < y) ? -1 : ((x > y) ? 1 : 0));
		} else { // else by score descending
			return score[b] - score[a];
		}
	});

	return results;
}

function getPageTitle(page) {
	return searchIndex.fileHeaders[page];
}

function getPageURL(page) {
	return searchIndex.filePaths[page];
}