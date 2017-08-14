using System;
using System.IO.Pipelines;
using System.IO.Pipelines.File;

namespace PlayingWithPipelines
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var factory = new PipeFactory())
			{
				var inputPipe = factory.ReadFile("data.txt");
				var outputPipe = factory.CreateWriter(Console.OpenStandardOutput());

				inputPipe.CopyToAsync(outputPipe).GetAwaiter().GetResult();
				inputPipe.Complete();

				outputPipe.Complete();

				Console.ReadLine();
			}
		}
	}
}