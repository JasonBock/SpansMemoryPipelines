using BenchmarkDotNet.Attributes;

namespace PlayingWithSpan
{
	[MemoryDiagnoser]
	public class BufferGeneratorPerformance
	{
		[Benchmark]
		public byte[] GetConverterBytes() =>
			BufferGenerators.GenerateWithConverters(
				22, new Coordinate { X = 10, Y = 20, Z = 30 },
				"This is a lot of string data. This is a lot of string data. This is a lot of string data.");

		[Benchmark]
		public byte[] GetSpanBytes() =>
			BufferGenerators.GenerateWithSpans(
				22, new Coordinate { X = 10, Y = 20, Z = 30 },
				"This is a lot of string data. This is a lot of string data. This is a lot of string data.");
	}
}
