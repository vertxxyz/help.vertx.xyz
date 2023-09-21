using System;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Troubleshooter.Tests;

internal static class AssertionExtensions
{
	[AttributeUsage(AttributeTargets.Parameter)]
	private sealed class ValidatedNotNullAttribute : Attribute { }

	public static void ThrowIfArgumentIsNull<T>([ValidatedNotNull] T obj, string paramName, string message)
	{
		if (obj is null)
		{
			throw new ArgumentNullException(paramName, message);
		}
	}

	public static StringAssertionsExtensions Should(this string instance) => new(instance);
	public static FileAssertions Should(this FileInfo instance) => new(instance);

	public static DirectoryAssertions Should(this DirectoryInfo instance) => new(instance);
}

internal class StringAssertionsExtensions : StringAssertions
{
	private const int TruncationLength = 100;
	private readonly string _truncated;

	public StringAssertionsExtensions(string value) : base(value)
	{
		if (string.IsNullOrEmpty(value))
			_truncated = value;
		else
			_truncated = value.Length <= TruncationLength ? value : $"{value[..(TruncationLength - 3)]}...";
	}

	/// <summary>
	/// Asserts that a string does not contain another (fragment of a) string.
	/// </summary>
	/// <param name="unexpected">
	/// The (fragment of a) string that the current string should not contain.
	/// </param>
	/// <param name="comparison">Comparison used for string.Contains</param>
	/// <param name="because">
	/// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
	/// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
	/// </param>
	/// <param name="becauseArgs">
	/// Zero or more objects to format using the placeholders in <paramref name="because" />.
	/// </param>
	public AndConstraint<StringAssertions> NotContain(string unexpected, StringComparison comparison, string because = "",
		params object[] becauseArgs)
	{
		AssertionExtensions.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot assert string containment against <null>.");

		if (unexpected.Length == 0)
		{
			throw new ArgumentException("Cannot assert string containment against an empty string.", nameof(unexpected));
		}

		Execute.Assertion
			.ForCondition(!Contains(Subject, unexpected, comparison))
			.BecauseOf(because, becauseArgs)
			.FailWith("Did not expect {context:string} to contain {1}{reason}.\n{0}", _truncated, unexpected);

		return new AndConstraint<StringAssertions>(this);
	}

	/// <summary>
	/// Asserts that a string does not contain another (fragment of a) string.
	/// </summary>
	/// <param name="unexpected">
	/// The (fragment of a) string that the current string should not contain.
	/// </param>
	/// <param name="comparison">Comparison used for string.Contains</param>
	/// <param name="because">
	/// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
	/// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
	/// </param>
	/// <param name="becauseArgs">
	/// Zero or more objects to format using the placeholders in <paramref name="because" />.
	/// </param>
	public AndConstraint<StringAssertions> NotContain(char unexpected, StringComparison comparison, string because = "",
		params object[] becauseArgs)
	{
		AssertionExtensions.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot assert string containment against <null>.");

		Execute.Assertion
			.ForCondition(!Contains(Subject, unexpected, comparison))
			.BecauseOf(because, becauseArgs)
			.FailWith("Did not expect {context:string} to contain {1}{reason}.\n{0}", _truncated, unexpected);

		return new AndConstraint<StringAssertions>(this);
	}

	private static bool Contains(string? actual, string? expected, StringComparison comparison) => (actual ?? string.Empty).Contains(expected ?? string.Empty, comparison);

	private static bool Contains(string? actual, char expected, StringComparison comparison) => (actual ?? string.Empty).Contains(expected, comparison);

	/// <summary>
	/// Asserts that a string does not match the regex.
	/// </summary>
	public AndConstraint<StringAssertions> NotMatch(Regex regex, string because = "", params object[] becauseArgs)
	{
		AssertionExtensions.ThrowIfArgumentIsNull(regex, nameof(regex), "Cannot match string against <null>. Provide valid regex.");

		Execute.Assertion
			.ForCondition(!regex.IsMatch(Subject))
			.BecauseOf(because, becauseArgs)
			.FailWith("Regex {1} matched {0}{reason}.", _truncated, regex);

		return new AndConstraint<StringAssertions>(this);
	}

	/// <summary>
	/// Asserts that a string does matches the regex.
	/// </summary>
	public AndConstraint<StringAssertions> Match(Regex regex, string because = "", params object[] becauseArgs)
	{
		AssertionExtensions.ThrowIfArgumentIsNull(regex, nameof(regex), "Cannot match string against <null>. Provide valid regex.");

		Execute.Assertion
			.ForCondition(regex.IsMatch(Subject))
			.BecauseOf(because, becauseArgs)
			.FailWith("Regex {1} matched {0}{reason}.", _truncated, regex);

		return new AndConstraint<StringAssertions>(this);
	}
}

internal class FileAssertions : FileAssertions<FileAssertions>
{
	public FileAssertions(FileInfo subject) : base(subject) { }
}

internal class FileAssertions<TAssertions> : ReferenceTypeAssertions<FileInfo, TAssertions>
	where TAssertions : FileAssertions<TAssertions>
{
	public FileAssertions(FileInfo subject) : base(subject) { }
	protected override string Identifier => nameof(FileInfo);

	/// <summary>
	/// Asserts that a File exists.
	/// </summary>
	/// <param name="because">
	/// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
	/// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
	/// </param>
	/// <param name="becauseArgs">
	/// Zero or more objects to format using the placeholders in <paramref name="because" />.
	/// </param>
	public AndConstraint<TAssertions> Exist(string because = "", params object[] becauseArgs)
	{
		Execute.Assertion
			.ForCondition(Subject.Exists)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {0} to exist at {1}{reason}.", Subject.Name, Subject.DirectoryName);

		return new AndConstraint<TAssertions>((TAssertions)this);
	}
}

internal class DirectoryAssertions : DirectoryAssertions<DirectoryAssertions>
{
	public DirectoryAssertions(DirectoryInfo subject) : base(subject) { }
}

internal class DirectoryAssertions<TAssertions> : ReferenceTypeAssertions<DirectoryInfo, TAssertions>
	where TAssertions : DirectoryAssertions<TAssertions>
{
	public DirectoryAssertions(DirectoryInfo subject) : base(subject) { }
	protected override string Identifier => nameof(DirectoryInfo);

	/// <summary>
	/// Asserts that a Directory exists.
	/// </summary>
	/// <param name="because">
	/// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
	/// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
	/// </param>
	/// <param name="becauseArgs">
	/// Zero or more objects to format using the placeholders in <paramref name="because" />.
	/// </param>
	public AndConstraint<TAssertions> Exist(string because = "", params object[] becauseArgs)
	{
		Execute.Assertion
			.ForCondition(Subject.Exists)
			.BecauseOf(because, becauseArgs)
			.FailWith("Expected {0} to exist {reason}.", Subject.Name);

		return new AndConstraint<TAssertions>((TAssertions)this);
	}
}
