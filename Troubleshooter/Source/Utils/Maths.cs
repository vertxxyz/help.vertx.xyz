// using System;

using System;

namespace Troubleshooter;

public static class Maths
{
	// private static readonly bool IsFlushToZeroEnabled = float.Epsilon == 0;
	// public static readonly float Epsilon = IsFlushToZeroEnabled ? 1.17549435E-38f : float.Epsilon;

	public static float Clamp01(float value) =>
		value switch
		{
			< 0 => 0,
			> 1 => 1,
			_ => value
		};

	public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp01(t);
	public static float LerpUnclamped(float a, float b, float t) => a + (b - a) * t;

	public static float ExponentialDecay(float a, float b, float decay, float deltaTime) => LerpUnclamped(a, b, 1 - (float)Math.Exp(-decay * deltaTime));

	public static float FractionalDamping(float a, float b, float fraction, float deltaTime) => LerpUnclamped(a, b, 1 - (float)Math.Pow(fraction, deltaTime));

	// public static bool Approximately(float a, float b) => Math.Abs(b - a) < Math.Max(0.000001f * Math.Max(Math.Abs(a), Math.Abs(b)), Epsilon * 8);
}
