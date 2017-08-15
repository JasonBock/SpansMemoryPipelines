using System;
using System.IO.Pipelines;
using System.IO.Pipelines.Networking.Sockets;
using System.IO.Pipelines.Text.Primitives;
using System.Net;
using System.Threading.Tasks;

namespace PlayingWithPipelines.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var server = new SocketListener())
			{
				var endpoint = new IPEndPoint(IPAddress.Loopback, 5020);

				server.OnConnection(Program.Handle);
				server.Start(endpoint);

				Console.Out.WriteLine("Press return to stop...");
				Console.In.ReadLine();

				server.Stop();
			}
		}

		private static async Task Handle(IPipeConnection connection)
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

				Console.Out.WriteLine(request.GetUtf8String());

				var response = connection.Output.Alloc();
				response.Append(request);
				await response.FlushAsync();
				connection.Input.Advance(request.End);
			}
		}
	}
}