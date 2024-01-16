// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace Troubleshooter;

public partial class AddHtmlExtension(bool skipRemainingRules) : IRule
{
	[GeneratedRegex(@"(?!.*\.)^.*$")]
	private static partial Regex GetRegexWithNoExtensions();

	private static readonly Regex s_regexWithNoExtensions = GetRegexWithNoExtensions();

	public void ApplyRule(RewriteContext context)
	{
		HttpRequest request = context.HttpContext.Request;

		if (request.Method != "GET")
			return;

		if (!s_regexWithNoExtensions.IsMatch(request.Path))
			return;

		if (skipRemainingRules)
			context.Result = RuleResult.SkipRemainingRules;

		request.Path = request.Path == "/" ? "/index.html" : $"{request.Path}.html";
	}
}
