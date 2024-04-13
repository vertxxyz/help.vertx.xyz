// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Troubleshooter;

public static class IndexHtml
{
	public static string GetWithContent(
		string content,
		string? sidebarContent = null
	)
	{
		sidebarContent ??= "<!-- sidebar content -->";
		// language=html
		return $"""
		        <!DOCTYPE html>
		        <html lang="en" class="rider-dark">
		        <head>
		            <meta charset="utf-8">
		            <meta name="viewport" content="width=device-width,user-scalable=no,minimum-scale=1,maximum-scale=1">
		            <meta name="robots" content="noai, noimageai">
		            <title>Unity, huh, how?</title>
		            <!--suppress HtmlUnknownTarget -->
		            <link rel="stylesheet" href="/Styles/style.css?v=2.5.0">
		            <!--suppress HtmlUnknownTarget -->
		            <link rel="stylesheet" href="/Styles/code-highlighting.css?v=1.0.3">
		            <link rel="preconnect" href="https://fonts.googleapis.com">
		            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
		            <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;700&family=Roboto:wght@400;700&subset=latin,latin-ext&display=swap" rel="stylesheet">
		            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/katex@0.16.9/dist/katex.min.css" integrity="sha384-n8MVd4RsNIU0tAv4ct0nTaAbDJwPJzDEaqSD1odI+WdtXRGWt2kTvGFasHpSy3SV" crossorigin="anonymous">
		            <link rel="icon" type="image/png" href="/Images/favicon-16x16.png" sizes="16x16">
		            <link rel="icon" type="image/png" href="/Images/favicon-32x32.png" sizes="32x32">
		            <link rel="icon" type="image/png" href="/Images/favicon-96x96.png" sizes="96x96">
		            <script src="/Scripts/core.js?v=1.2.0"></script>
		            <script src="/Scripts/script.js?v=1.7.1"></script>
		            <script src="/Scripts/sidebar.js?v=1.2.0"></script>
		            <script src="/Scripts/search.js?v=1.3.0"></script>
		            <script src="/Scripts/tableHighlighter.js?v=1.0.1"></script>
		        </head>
		        <body>
		        <div class="hidden">
		            <svg xmlns="http://www.w3.org/2000/svg">
		                <symbol id="sidebar-expand-icon" viewBox="500 0 400 400" xml:space="preserve">
		                <path d="M700,100c-9.2,0-16.7,7.5-16.7,16.7s7.5,16.7,16.7,16.7h100c9.2,0,16.7-7.5,16.7-16.7S809.2,100,800,100H700z  "></path>
		                    <path d="M586.1,190.8c-0.3,0.4-0.4,1-0.6,1.5c-0.5,0.9-0.9,1.9-1.2,2.9c-0.2,0.6-0.5,1.1-0.7,1.7  c-0.1,0.6,0,1.1,0,1.7c0,0.5-0.3,1-0.3,1.5s0.3,1,0.3,1.5c0.1,0.6-0.1,1.1,0,1.7c0.1,0.6,0.5,1.1,0.7,1.7c0.3,1,0.7,2,1.2,2.9  c0.3,0.5,0.3,1,0.6,1.5c0,0,0,0,0,0l33.3,50c3.2,4.8,8.5,7.4,13.9,7.4c3.2,0,6.4-0.9,9.2-2.8c7.7-5.1,9.7-15.5,4.6-23.1l-16.1-24.1  H800c9.2,0,16.7-7.5,16.7-16.7s-7.5-16.7-16.7-16.7H631.1l16.1-24.1c5.1-7.7,3-18-4.6-23.1c-7.7-5.1-18-3-23.1,4.6L586.1,190.8  C586.1,190.8,586.1,190.8,586.1,190.8z"></path>
		                    <path d="M800,266.7H700c-9.2,0-16.7,7.5-16.7,16.7S690.8,300,700,300h100c9.2,0,16.7-7.5,16.7-16.7  S809.2,266.7,800,266.7z"></path>
		                </symbol>
		                <symbol id="code-expand-icon"
		                        style="width: 1em; height: 1em;vertical-align: middle;overflow: hidden;"
		                        viewBox="0 0 1024 1024">
		                    <path d="M512 581.5l273.1-273.1c25-25 65.5-25 90.5 0s25 65.5 0 90.5l-316.7 316.7c-12.9 12.9-30 19.2-47 18.7-16.9 0.4-33.9-5.8-46.8-18.699l-316.8-316.8c-25-25-25-65.5 0-90.5s65.5-25 90.5 0l273.2 273.2z"/>
		                </symbol>
		                <symbol id="menu-path-separator-icon"
		                        style="width: 1em; height: 1em;vertical-align: middle;overflow: hidden;"
		                        viewBox="0 0 1024 1024">
		                    <path d="M512 581.5l273.1-273.1c25-25 65.5-25 90.5 0s25 65.5 0 90.5l-316.7 316.7c-12.9 12.9-30 19.2-47 18.7-16.9 0.4-33.9-5.8-46.8-18.699l-316.8-316.8c-25-25-25-65.5 0-90.5s65.5-25 90.5 0l273.2 273.2z"/>
		                </symbol>
		            </svg>
		        </div>
		        <div id="local-developer-tools" class="header dev-tools-header hidden">
		            <div class="interactive-button" onclick="postText('tools', 'rebuild-all')">Rebuild All</div>
		            <div class="interactive-button" onclick="postText('tools', 'rebuild-content')">Rebuild Content</div>
		        </div>
		        <div class="header">
		            <div class="header__contents">
		                <div class="header__title header__title--large">
		                    <a class="" href="/"><img class="emoji" draggable="false" alt="🤔"
		                                              src="/Images/favicon-96x96.png">
		                        Unity, huh, how?
		                    </a></div>
		                <div class="header__title header__title--small">
		                    <a class="" href="/"><img class="emoji" draggable="false" alt="🤔"
		                                              src="/Images/favicon-96x96.png"></a>
		                </div>
		            </div>
		            <div class="header__sidebar">
		                <input class="header__search" id="page-search" type="text" placeholder="Search...">
		                <button class="sidebar__button" type="button" id="button-sidebar" title="Toggle sidebar">
		                    <svg xmlns="http://www.w3.org/2000/svg" class="sidebar__button__content" viewBox="0 0 400 400">
		                        <use href="#sidebar-expand-icon"></use>
		                    </svg>
		                </button>
		            </div>
		        </div>
		        <div id="container" class="container">
		            <div id="contents" class="contents">
		                {content}
		            </div>
		            <div class="nav_overlay"></div>
		            <div class="sidebar">
		                <div id="sidebar-contents" class="sidebar-contents">
		                    {sidebarContent}
		                </div>
		                <div class="sidebar-bottom">
		                    <a id="report-link" class="report-link light-link link--external" onclick="reportIssue()" target="_blank">Report an issue
		                        with this page</a>
		                    <a href="https://ko-fi.com/vertx" id="kofi-link" class="light-link link--external">Support me on Ko-fi</a>
		                </div>
		            </div>
		        </div>
		        </body>
		        </html>
		        """;
	}
}
