using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Troubleshooter;

// ReSharper disable once InconsistentNaming
public enum CDN
{
	JsDelivr = 0,
	Unpkg = 1,
}

[Flags]
// ReSharper disable once InconsistentNaming
public enum SupportedCDNs
{
	None,
	JsDelivr = 1 << CDN.JsDelivr,
	Unpkg = 1 << CDN.Unpkg,
	All = JsDelivr | Unpkg
}

public sealed class OnlineResource
{
	public string UnpkgUri { get; }

	public string JsDelivrUri { get; }

	// ReSharper disable once InconsistentNaming
	public SupportedCDNs CDNs { get; }

	public string FileContent { get; private set; }

	private readonly string _owner, _path;

	public OnlineResource(string owner, string path, SupportedCDNs cdns)
	{
		_owner = owner;
		_path = path;
		UnpkgUri = $"https://unpkg.com/{owner}@latest/dist/{path}";
		JsDelivrUri =
			string.IsNullOrEmpty(owner)
				? $"https://cdn.jsdelivr.net/npm/{path}@latest"
				: $"https://cdn.jsdelivr.net/npm/{owner}@latest/dist/{path}";
		CDNs = cdns;
		FileContent = "";
	}

	public static implicit operator string(OnlineResource resource) => resource.FileContent;

	public async Task RequestFileContent(HttpClient c, CDN cdn)
	{
		// Already loaded
		if (!string.IsNullOrEmpty(FileContent))
			return;

		// Check requested CDN is valid, and fall back if needed.
		if ((CDNs & (SupportedCDNs)(1 << (int)cdn)) == 0)
		{
			// Fall back to supported CDN. This logic can all be rewritten if there's ever more than 2.
			if ((CDNs & SupportedCDNs.JsDelivr) != 0)
				cdn = CDN.JsDelivr;
			else
				cdn = CDN.Unpkg;
		}

		string uri = cdn switch
		{
			CDN.Unpkg => UnpkgUri,
			CDN.JsDelivr => JsDelivrUri,
			_ => throw new ArgumentOutOfRangeException(nameof(cdn), cdn, null)
		};

		const int maxRetries = 4;
		int retries = 0;
		do
		{
			try
			{
				HttpResponseMessage message = await c.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
				if (message.IsSuccessStatusCode)
				{
					Console.WriteLine($"\"{_owner}/{_path}\" loaded successfully.");
					FileContent = await message.Content.ReadAsStringAsync();
					return;
				}

				Console.WriteLine($"\"{uri}\" was not found when attempting to load {nameof(OnlineResource)}, retrying. {message.StatusCode}: {message.RequestMessage}.");
			}
			catch (TimeoutException)
			{
				Console.WriteLine($"\"{uri}\" timed out when attempting to load {nameof(OnlineResource)}, retrying.");
			}
		} while (retries++ < maxRetries);

		throw new BuildException($"{uri} could not be loaded.");
	}
}

public class OnlineResources
{
	public readonly OnlineResource Graphre = new("graphre", "graphre.js", SupportedCDNs.All);
	public readonly OnlineResource D3 = new("", "d3", SupportedCDNs.JsDelivr);
	public readonly OnlineResource Plot = new("", "@observablehq/plot", SupportedCDNs.JsDelivr);
	public readonly OnlineResource Mermaid = new("mermaid", "mermaid.min.js", SupportedCDNs.All);
	public readonly OnlineResource KaTeX = new("katex", "katex.min.js", SupportedCDNs.All);
	public readonly OnlineResource Nomnoml = new("nomnoml", "nomnoml.js", SupportedCDNs.All);

	public async Task LoadAll()
	{
		Ping ping = new();

		try
		{
			CDN chosenCdn = await GetFirstConnectedCDN();

			using var httpClient = new HttpClient();

			httpClient.Timeout = TimeSpan.FromSeconds(10);
			await Task.WhenAll(
				Graphre.RequestFileContent(httpClient, chosenCdn),
				D3.RequestFileContent(httpClient, chosenCdn),
				Plot.RequestFileContent(httpClient, chosenCdn),
				Mermaid.RequestFileContent(httpClient, chosenCdn),
				KaTeX.RequestFileContent(httpClient, chosenCdn),
				Nomnoml.RequestFileContent(httpClient, chosenCdn)
			);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return;

		async Task<CDN> GetFirstConnectedCDN()
		{
			// Ping CDN services, quickest to reply wins.
			Task<PingReply> pingUnpkg = ping.SendPingAsync("https://unpkg.com/");
			Task<PingReply> pingJsDelivr = ping.SendPingAsync("https://cdn.jsdelivr.net/");

			Task<PingReply> reply = await Task.WhenAny(
				pingUnpkg,
				pingJsDelivr
			);

			return reply switch
			{
				_ when reply == pingUnpkg => CDN.Unpkg,
				_ when reply == pingJsDelivr => CDN.JsDelivr,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}
