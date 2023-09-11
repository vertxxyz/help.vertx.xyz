namespace Troubleshooter;

public interface IProcessorGroup
{
	PageResourcesPostProcessors ResourceProcessors { get; }
	MarkdownPreProcessors PreProcessors { get; }
	HtmlPostProcessors PostProcessors { get; }
}

public sealed class ProcessorsGroup : IProcessorGroup
{
	public PageResourcesPostProcessors ResourceProcessors { get; }
	public MarkdownPreProcessors PreProcessors { get; }
	public HtmlPostProcessors PostProcessors { get; }

	public ProcessorsGroup(
		PageResourcesPostProcessors resourceProcessors,
		MarkdownPreProcessors preProcessors,
		HtmlPostProcessors postProcessors
	)
	{
		ResourceProcessors = resourceProcessors;
		PreProcessors = preProcessors;
		PostProcessors = postProcessors;
	}
}