using System;
using System.IO.Pipelines;
using System.IO.Pipelines.File;
using System.Threading.Tasks;

namespace PlayingWithPipelines
{
	class Program
	{
		static async Task Main(string[] args)
		{
			using (var factory = new PipeFactory())
			{
				var inputPipe = factory.ReadFile("data.txt");
				var outputPipe = factory.CreateWriter(Console.OpenStandardOutput());

				await inputPipe.CopyToAsync(outputPipe);
				inputPipe.Complete();
				outputPipe.Complete();

				Console.ReadLine();
			}
		}
	}
}