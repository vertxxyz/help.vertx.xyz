namespace Troubleshooter
{
	public interface IHtmlPostProcessor
	{
		string Process(string html);
		int Order => 0;
	}
}