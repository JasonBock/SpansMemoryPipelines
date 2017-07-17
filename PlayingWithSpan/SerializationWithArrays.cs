using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PlayingWithSpan
{
	[MemoryDiagnoser]
	public class SerializationWithArrays
	{
		private byte[] converterData;
		private byte[] spanData;

		[GlobalSetup]
		public void Setup()
		{
			this.converterData = BufferGenerators.GenerateWithConverters(
				22, new Coordinate { X = 10, Y = 20, Z = 30 },
				"This is a lot of string data. This is a lot of string data. This is a lot of string data.");
			this.spanData = BufferGenerators.GenerateWithSpans(
				22, new Coordinate { X = 10, Y = 20, Z = 30 },
				"This is a lot of string data. This is a lot of string data. This is a lot of string data.");
		}

		[Benchmark]
		public int GetConverterValue()
		{
			var pointSize = Marshal.SizeOf<Coordinate>();

			var value = BitConverter.ToInt32(this.converterData, 0);
			var pointPtr = Marshal.AllocHGlobal(pointSize);
			Marshal.Copy(this.converterData, 4, pointPtr, pointSize);
			var coordinate = (Coordinate)Marshal.PtrToStructure(pointPtr, typeof(Coordinate));
			Marshal.FreeHGlobal(pointPtr);

			var data = Encoding.Unicode.GetString(this.converterData, 
				4 + pointSize, this.converterData.Length - (4 + pointSize));

			return value + coordinate.Y + data.Length;
		}

		[Benchmark]
		public int GetSpanValue()
		{
			var pointSize = Marshal.SizeOf<Coordinate>();
			var span = this.converterData.AsSpan();

			var value = span.Slice(0, 4).NonPortableCast<byte, int>()[0];
			var coordinate = span.Slice(4, pointSize).NonPortableCast<byte, Coordinate>()[0];
			var data = new string(span.Slice(4 + pointSize).NonPortableCast<byte, char>().ToArray());

			return value + coordinate.Y + data.Length;
		}
	}
}
