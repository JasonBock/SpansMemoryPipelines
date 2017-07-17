using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PlayingWithSpan
{
	internal static class BufferGenerators
	{
		internal static byte[] GenerateWithConverters(int value, Coordinate point, string data)
		{
			var valueArray = BitConverter.GetBytes(value);

			var pointSize = Marshal.SizeOf(point);
			var pointArray = new byte[pointSize];
			var pointPtr = Marshal.AllocHGlobal(pointSize);

			Marshal.StructureToPtr(point, pointPtr, true);
			Marshal.Copy(pointPtr, pointArray, 0, pointSize);
			Marshal.FreeHGlobal(pointPtr);

			var dataArray = Encoding.Unicode.GetBytes(data);

			var buffer = new List<byte>();
			buffer.AddRange(valueArray);
			buffer.AddRange(pointArray);
			buffer.AddRange(dataArray);

			return buffer.ToArray();
		}

		internal static byte[] GenerateWithSpans(int value, Coordinate point, string data)
		{
			var valueArray = new Span<int>(new[] { value }).NonPortableCast<int, byte>().ToArray();
			var pointArray = new Span<Coordinate>(new[] { point }).NonPortableCast<Coordinate, byte>().ToArray();
			var dataArray = data.AsSpan().NonPortableCast<char, byte>().ToArray();

			var buffer = new List<byte>();
			buffer.AddRange(valueArray);
			buffer.AddRange(pointArray);
			buffer.AddRange(dataArray);

			return buffer.ToArray();
		}
	}
}
