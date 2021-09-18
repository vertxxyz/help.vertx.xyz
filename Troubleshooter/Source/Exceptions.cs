using System;

namespace Troubleshooter
{
	public class BuildException : Exception
	{
		public BuildException(string message = "") : base(message) { }
		public BuildException(Exception exception, string message = "") : base(message, exception) { }
	}

	public class LogicException : Exception
	{
		public LogicException(string message = "") : base(message) { }
	}
}