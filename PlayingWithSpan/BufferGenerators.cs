using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PlayingWithSpan
{
	internal static class BufferGenerators
	{
		internal static byte[] GenerateWithConverters(int value, Coordinate coordinate, string data)
		{
			var valueArray = BitConverter.GetBytes(value);

			var pointSize = Marshal.SizeOf(coordinate);
			var pointArray = new byte[pointSize];
			var pointPtr = Marshal.AllocHGlobal(pointSize);

			Marshal.StructureToPtr(coordinate, pointPtr, true);
			Marshal.Copy(pointPtr, pointArray, 0, pointSize);
			Marshal.FreeHGlobal(pointPtr);

			var dataArray = Encoding.Unicode.GetBytes(data);

			var buffer = new List<byte>();
			buffer.AddRange(valueArray);
			buffer.AddRange(pointArray);
			buffer.AddRange(dataArray);

			return buffer.ToArray();
		}

		internal static byte[] GenerateWithStream(int value, Coordinate coordinate, string data)
		{
			using (var writer = new BinaryWriter(new MemoryStream(), Encoding.Unicode))
			{
				writer.Write(value);
				writer.Write(coordinate.X);
				writer.Write(coordinate.Y);
				writer.Write(coordinate.Z);
				writer.Write(data);
				writer.Flush();
				var stream = writer.BaseStream;
				var buffer = new byte[stream.Length];
				stream.Read(buffer, 0, (int)stream.Length);
				return buffer;
			}
		}

		internal static byte[] GenerateWithSpan(int value, Coordinate coordinate, string data)
		{
			var valueArray = new Span<int>(new[] { value }).NonPortableCast<int, byte>().ToArray();
			var pointArray = new Span<Coordinate>(new[] { coordinate }).NonPortableCast<Coordinate, byte>().ToArray();
			var dataArray = data.AsSpan().NonPortableCast<char, byte>().ToArray();

			var buffer = new List<byte>();
			buffer.AddRange(valueArray);
			buffer.AddRange(pointArray);
			buffer.AddRange(dataArray);

			return buffer.ToArray();
		}
	}
}
