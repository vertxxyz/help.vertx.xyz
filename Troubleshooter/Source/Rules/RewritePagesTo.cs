using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace Troubleshooter;

public sealed partial class RewritePagesTo(string redirectTo, bool skipRemainingRules) : IRule
{
	[GeneratedRegex(@"(?!.*\.)^.*$")]
	private static partial Regex RegexWithNoExtensions { get; }

	public void ApplyRule(RewriteContext context)
	{
		HttpRequest request = context.HttpContext.Request;

		if (request.Method != "GET")
			return;

		if (!RegexWithNoExtensions.IsMatch(request.Path))
			return;

		if (skipRemainingRules)
			context.Result = RuleResult.SkipRemainingRules;

		request.Path = redirectTo;
	}
}
