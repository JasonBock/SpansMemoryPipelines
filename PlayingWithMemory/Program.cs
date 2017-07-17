using BenchmarkDotNet.Running;
using System;

namespace PlayingWithMemory
{
	class Program
	{
		static void Main(string[] args) =>
			//Program.NaiveRefReturnsExample();
			BenchmarkRunner.Run<ListIndexPerformance>();

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