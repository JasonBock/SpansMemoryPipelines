using BenchmarkDotNet.Running;
using System;

namespace PlayingWithMemory
{	
	class Program
	{
		static void Main(string[] args) =>
			//Program.NaiveRefReturnsExample();
			//BenchmarkRunner.Run<ListIndexPerformance>();
			//BenchmarkRunner.Run<ScalingTests>();
			//BenchmarkRunner.Run<AllocatingMemory>();
			Program.BadCode();

		private static unsafe void BadCode()
		{
			var unsafeBuffer = stackalloc byte[100];
			Console.Out.WriteLine(unsafeBuffer[200]);

			var safeBuffer = new byte[100];
			Console.Out.WriteLine(safeBuffer[200]);
		}

		private static void NaiveRefReturnsExample()
		{
			var returns = new RefReturns();

			ref var data1 = ref returns.GetIndividualData(3);
			ref var data2 = ref returns.GetIndividualData(3);

			data2 = 20;
			Console.Out.WriteLine(data1);
		}
	}
}