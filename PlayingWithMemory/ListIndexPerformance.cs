using BenchmarkDotNet.Attributes;
using System;

namespace PlayingWithMemory
{
	[MemoryDiagnoser]
	public class ListIndexPerformance
	{
		private SlowList<GiantStruct> slowListData;
		private FastList<GiantStruct> fastListData;

		[GlobalSetup]
		public void Setup()
		{
			this.slowListData = new SlowList<GiantStruct>(1);
			this.slowListData[0] = new GiantStruct { Value1 = 222, Value2 = Guid.NewGuid(), Value3 = 22.2, Value4 = "A bunch of data" };
			this.fastListData = new FastList<GiantStruct>(1);
			this.fastListData[0] = new GiantStruct { Value1 = 222, Value2 = Guid.NewGuid(), Value3 = 22.2, Value4 = "A bunch of data" };
		}

		[Benchmark]
		public int GetNameFromFastList() => ListIndexPerformance.GetStringValueLength(this.fastListData[0].Value4);

		[Benchmark]
		public int GetNameFromSlowList() => ListIndexPerformance.GetStringValueLength(this.slowListData[0].Value4);

		private static int GetStringValueLength(string value) => value.Length;
	}
}
