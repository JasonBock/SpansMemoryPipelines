using System;
using System.IO.Pipelines;
using System.IO.Pipelines.File;
using System.IO.Pipelines.Networking.Sockets;
using System.IO.Pipelines.Text.Primitives;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Formatting;

namespace PlayingWithPipelines
{
	class Program
	{
		static void Main(string[] args) =>
			//Program.SimplePipe();
			Program.ConnectionExampleAsync().GetAwaiter().GetResult();

		private static void SimplePipe()
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

		private static async Task ConnectionExampleAsync()
		{
			var endpoint = new IPEndPoint(IPAddress.Loopback, 5020);

			const string MessageToSend = "Hello world!";
			string reply = null;

			using (var server = new SocketListener())
			{
				server.OnConnection(Echo);
				server.Start(endpoint);

				using (var client = await SocketConnection.ConnectAsync(endpoint))
				{
					try
					{
						var output = client.Output.Alloc();
						output.Append(MessageToSend, SymbolTable.InvariantUtf8);
						await output.FlushAsync();
						client.Output.Complete();

						while (true)
						{
							var result = await client.Input.ReadAsync();

							var input = result.Buffer;

							// wait for the end of the data before processing anything
							if (result.IsCompleted)
							{
								reply = input.GetUtf8String();
								client.Input.Advance(input.End);
								break;
							}
							else
							{
								client.Input.Advance(input.Start, input.End);
							}
						}
					}
					finally
					{
						await client.DisposeAsync();
					}
				}
			}

			Console.Out.WriteLine($"{MessageToSend} - {reply}");
		}

		private static async Task Echo(IPipeConnection connection)
		{
			while (true)
			{
				var result = await connection.Input.ReadAsync();
				var request = result.Buffer;

				if (request.IsEmpty && result.IsCompleted)
				{
					connection.Input.Advance(request.End);
					break;
				}

				var response = connection.Output.Alloc();
				response.Append(request);
				await response.FlushAsync();
				connection.Input.Advance(request.End);
			}
		}
	}
}