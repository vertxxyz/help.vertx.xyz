using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Converts a slider div to have a complete hierarchy required for consistent styling.
/// </summary>
[UsedImplicitly]
public sealed partial class SliderConverter : IHtmlPostProcessor
{
	[GeneratedRegex("<div.* class=\".*?slider\"></div>")]
	private static partial Regex SliderRegex { get; }

	public string Process(string html, string fullPath)
	{
		return StringUtility.ReplaceMatch(html, SliderRegex, (group, stringBuilder) =>
		{
			stringBuilder.Append(group[..^6]);
			{
				stringBuilder.Append("<div class=\"slider_container\">");
				stringBuilder.Append("<div class=\"slider_left_gutter\"></div>");
				stringBuilder.Append("<div class=\"slider_right_gutter\"></div>");
				stringBuilder.Append("<div class=\"slider_knob_container\">");
				stringBuilder.Append("<div class=\"slider_knob\"></div>");
				stringBuilder.Append("</div>");
				stringBuilder.Append("</div>");
				//<div class="slider_container">
				//	<div class="slider_left_gutter"></div>
				//	<div class="slider_right_gutter"></div>
				//	<div class="slider_knob_container">
				//		<div class="slider_knob"></div>
				//	</div>
				//</div>
			}
			stringBuilder.Append("</div>");
		}, 0);
	}
}
