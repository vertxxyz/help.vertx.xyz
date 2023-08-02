using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public sealed partial class SliderConverter : IHtmlPostProcessor
{
	[GeneratedRegex("<div.* class=\".*?slider\"></div>", RegexOptions.Compiled)]
	private static partial Regex GetSliderRegex();

	private static readonly Regex s_SliderRegex = GetSliderRegex();

	public string Process(string html, string fullPath)
	{
		return StringUtility.ReplaceMatch(html, s_SliderRegex, (group, stringBuilder) =>
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