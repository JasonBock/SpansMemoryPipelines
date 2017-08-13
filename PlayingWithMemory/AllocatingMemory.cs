using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;

namespace PlayingWithMemory
{
	[MemoryDiagnoser]
	public class AllocatingMemory
	{
		[Params(10, 50, 100, 500, 1000, 5000, 10000, 50000)]
		public int Size { get; set; }

		[Benchmark]
		public byte AllocateWithMarshal()
		{
			var buffer = Marshal.AllocHGlobal(this.Size);
			var result = Marshal.ReadByte(buffer, this.Size / 2);
			Marshal.FreeHGlobal(buffer);
			return result;
		}

		[Benchmark]
		public unsafe byte AllocateWithPointers()
		{
			var buffer = stackalloc byte[this.Size];
			return buffer[this.Size / 2];
		}

		[Benchmark]
		public byte AllocateWithNew()
		{
			var buffer = new byte[this.Size];
			return buffer[this.Size / 2];
		}
	}
}
