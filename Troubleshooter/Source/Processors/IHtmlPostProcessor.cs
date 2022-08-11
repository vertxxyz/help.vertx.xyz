namespace Troubleshooter;

public interface IHtmlPostProcessor
{
	string Process(string html, string fullPath);
	int Order => 0;
}