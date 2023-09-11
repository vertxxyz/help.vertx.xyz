using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace Troubleshooter;

public sealed partial class RewritePagesTo : IRule
{
	private readonly string _redirectTo;
	private readonly bool _skipRemainingRules;

	[GeneratedRegex(@"(?!.*\.)^.*$")]
	private static partial Regex GetRegexWithNoExtensions();

	private static readonly Regex s_RegexWithNoExtensions = GetRegexWithNoExtensions();

	public RewritePagesTo(string redirectTo, bool skipRemainingRules)
	{
		_redirectTo = redirectTo;
		_skipRemainingRules = skipRemainingRules;
	}

	public void ApplyRule(RewriteContext context)
	{
		HttpRequest request = context.HttpContext.Request;

		if (request.Method != "GET")
			return;

		if (!s_RegexWithNoExtensions.IsMatch(request.Path))
			return;

		if (_skipRemainingRules)
			context.Result = RuleResult.SkipRemainingRules;

		request.Path = _redirectTo;
	}
}