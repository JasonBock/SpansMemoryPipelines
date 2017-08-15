using System;
using System.IO.Pipelines;
using System.IO.Pipelines.Networking.Sockets;
using System.IO.Pipelines.Text.Primitives;
using System.Net;
using System.Text;
using System.Text.Formatting;
using System.Threading.Tasks;

namespace PlayingWithPipelines.Client
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var endpoint = new IPEndPoint(IPAddress.Loopback, 5020);

			while (true)
			{
				Console.Out.WriteLine("Enter in your message to echo...");
				var message = Console.In.ReadLine();

				using (var client = await SocketConnection.ConnectAsync(endpoint))
				{
					var output = client.Output.Alloc();
					output.Append(message, SymbolTable.InvariantUtf8);
					await output.FlushAsync();
					client.Output.Complete();

					while (true)
					{
						var result = await client.Input.ReadAsync();

						var input = result.Buffer;

						if (result.IsCompleted)
						{
							Console.Out.WriteLine(input.GetUtf8String());
							client.Input.Advance(input.End);
							break;
						}
						else
						{
							client.Input.Advance(input.Start, input.End);
						}
					}
				}
			}
		}
	}
}
