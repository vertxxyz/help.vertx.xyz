using System;

namespace Troubleshooter;

public sealed class BuildScope : IDisposable
{
	public BuildScope() => IOUtility.ResetRecording();

	public void Dispose() => IOUtility.ResetRecording();
}
