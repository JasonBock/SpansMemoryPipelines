using BenchmarkDotNet.Running;

namespace PlayingWithSpan
{
	class Program
	{
		static void Main(string[] args) =>
			//BenchmarkRunner.Run<SerializationWithArrays>();
			BenchmarkRunner.Run<FormattingTests>();
	}
}