using Troubleshooter.Constants;

namespace Troubleshooter;

public interface IPageResourcesPostProcessor
{
	void Process(PageResources dictionary, Site site);
}