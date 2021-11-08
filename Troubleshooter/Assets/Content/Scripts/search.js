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
fetch("Json/search-index.json")
	.then(response => response.json())
	.then(json => searchIndex = json);

$(document).ready(function () {
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
	const sidebarContents = $('.sidebar-contents');
	if(sidebarContents.length === 0) return;
	const value = processPageValue(getPageParameter(), false);
	sidebarContents.load(`HTML/${value}_sidebar.html`, function (response, status, xhr) {
		if (status === "error")
			sidebarContents.empty();
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
	const sidebarContents = $('.sidebar-contents');
	sidebarContents.empty();
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
	const common = searchIndex.common;
	const termLookup = searchIndex.termsToFileIndices;
	const score = {};
	let min_score = terms.length;
	for (var i = 0; i < terms.length; i++) {
		var term = terms[i];
		if (common[term]) {
			min_score--;
		}

		if (termLookup[term]) {
			for (var j = 0; j < termLookup[term].length; j++) {
				var page = termLookup[term][j];
				if (!score[page])
					score[page] = j;
				++score[page];
			}
		}

		for (var si in termLookup) {
			if (si.length <= term.length) continue;
			if (si.slice(0, term.length) === term) {
				for (var j = 0; j < termLookup[si].length; j++) {
					var page = termLookup[si][j];
					if (!score[page])
						score[page] = j;
					++score[page];
				}
			}
		}
	}
	var results = [];
	for (var page in score) {
		var title = getPageTitle(page);
		// ignore partial matches
		if (score[page] >= min_score) {
			results.push(page);

			var placement;
			// Adjust scores for better matches
			for (var i = 0; i < terms.length; i++) {
				var term = terms[i];
				if ((placement = title.toLowerCase().indexOf(term)) > -1) {
					score[page] += 50;
					if (placement === 0 || title[placement - 1] === '.')
						score[page] += 500;
					if (placement + term.length === title.length || title[placement + term.length] === '.')
						score[page] += 500;
				}
			}

			if (title.toLowerCase() === query)
				score[page] += 10000;
			else if ((placement = title.toLowerCase().indexOf(query)) > -1)
				score[page] += ((placement < 100) ? (200 - placement) : 100);
		}
	}

	results = results.sort(function (a, b) {
		if (score[b] === score[a]) { // sort alphabetically by title if score is the same
			var x = getPageTitle(a).toLowerCase();
			var y = getPageTitle(b).toLowerCase();
			return ((x < y) ? -1 : ((x > y) ? 1 : 0));
		} else { // else by score descending
			return score[b] - score[a]
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