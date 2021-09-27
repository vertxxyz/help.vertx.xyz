using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Troubleshooter.Tests
{
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
		
		public static StringAssertionsExtensions Should(this string instance) => new StringAssertionsExtensions(instance);
	}

	internal class StringAssertionsExtensions : StringAssertions
	{
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
				.FailWith("Did not expect {context:string} {0} to contain {1}{reason}.", Subject, unexpected);

			return new AndConstraint<StringAssertions>(this);
		}

		private static bool Contains(string actual, string expected, StringComparison comparison) => (actual ?? string.Empty).Contains(expected ?? string.Empty, comparison);

		public StringAssertionsExtensions(string value) : base(value) { }
	}
}