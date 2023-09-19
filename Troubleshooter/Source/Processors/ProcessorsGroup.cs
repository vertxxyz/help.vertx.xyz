namespace Troubleshooter;

public interface IProcessorGroup
{
	PageResourcesPostProcessors ResourceProcessors { get; }
	MarkdownPreProcessors PreProcessors { get; }
	HtmlPostProcessors PostProcessors { get; }
	BuildPostProcessors BuildPostProcessors { get; }
}

public sealed class ProcessorsGroup : IProcessorGroup
{
	public PageResourcesPostProcessors ResourceProcessors { get; }
	public MarkdownPreProcessors PreProcessors { get; }
	public HtmlPostProcessors PostProcessors { get; }
	public BuildPostProcessors BuildPostProcessors { get; }

	public ProcessorsGroup(
		PageResourcesPostProcessors resourceProcessors,
		MarkdownPreProcessors preProcessors,
		HtmlPostProcessors postProcessors,
		BuildPostProcessors buildPostProcessors
	)
	{
		ResourceProcessors = resourceProcessors;
		PreProcessors = preProcessors;
		PostProcessors = postProcessors;
		BuildPostProcessors = buildPostProcessors;
	}
}