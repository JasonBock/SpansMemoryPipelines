using BenchmarkDotNet.Attributes;
using Spackle;
using System.Linq;

namespace PlayingWithMemory
{
	[MemoryDiagnoser]
	public class ScalingTests
	{
		private const int Low = 50;
		private const int High = 75;
		private const int Scale = 200;

		private int[] data;

		[Params(10, 100, 1000, 10000)]
		public int ArraySize { get; set; }

		[GlobalSetup]
		public void GlobalSetup()
		{
			this.data = new int[this.ArraySize];

			var random = new SecureRandom();

			for (var i = 0; i < this.ArraySize; i++)
			{
				this.data[i] = random.Next(100);
			}
		}

		[Benchmark]
		public int[] ScaleWithLinq() =>
			(from x in this.data
			 where (x >= ScalingTests.Low) && (x <= ScalingTests.High)
			 select (x * ScalingTests.Scale)).ToArray();


		[Benchmark(Baseline = true)]
		public int[] ScaleWithoutLinq()
		{
			var scaled = new int[this.ArraySize];

			for (var i = 0; i < this.data.Length; i++)
			{
				if ((this.data[i] >= ScalingTests.Low) && (this.data[i] <= ScalingTests.High))
				{
					scaled[i] *= (this.data[i] >= ScalingTests.Low) && (this.data[i] <= ScalingTests.High) ?
						this.data[i] * ScalingTests.Scale : this.data[i];
				}
			}

			return scaled;
		}
	}
}
