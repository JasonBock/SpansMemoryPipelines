using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PlayingWithSpan
{
	class Program
	{
		static void Main(string[] args)
		{
			var encodingBuffer = Program.GenerateWithConverters(
				22, new Coordinate { X = 5, Y = 10, Z = 30 }, "This is some data");

			var spanBuffer = Program.GenerateWithSpans(
				22, new Coordinate { X = 5, Y = 10, Z = 30 }, "This is some data");

			Program.ConversionViaConverter(encodingBuffer);
			Program.ConversionViaSpans(spanBuffer);
		}

		private static void ConversionViaConverter(byte[] buffer)
		{
			var pointSize = Marshal.SizeOf<Coordinate>();

			var value = BitConverter.ToInt32(buffer, 0);
			var pointPtr = Marshal.AllocHGlobal(pointSize);
			Marshal.Copy(buffer, 4, pointPtr, pointSize);
			var coordinate = (Coordinate)Marshal.PtrToStructure(pointPtr, typeof(Coordinate));
			Marshal.FreeHGlobal(pointPtr);

			var data = Encoding.ASCII.GetString(buffer, 4 + pointSize, buffer.Length - (4 + pointSize));
			Console.Out.WriteLine($"Value is {value}, coordinate is ({coordinate.X}, {coordinate.Y}, {coordinate.Z}), data is {data}");
		}

		private static void ConversionViaSpans(Span<byte> buffer)
		{
			var pointSize = Marshal.SizeOf<Coordinate>();

			var value = buffer.Slice(0, 4).NonPortableCast<byte, int>()[0];
			var coordinate = buffer.Slice(4, pointSize).NonPortableCast<byte, Coordinate>()[0];
			var data = new string(buffer.Slice(4 + pointSize).NonPortableCast<byte, char>().ToArray());

			Console.Out.WriteLine($"Value is {value}, coordinate is ({coordinate.X}, {coordinate.Y}, {coordinate.Z}), data is {data}");
		}

		private static byte[] GenerateWithConverters(int value, Coordinate point, string data)
		{
			var valueArray = BitConverter.GetBytes(value);

			var pointSize = Marshal.SizeOf(point);
			var pointArray = new byte[pointSize];
			var pointPtr = Marshal.AllocHGlobal(pointSize);

			Marshal.StructureToPtr(point, pointPtr, true);
			Marshal.Copy(pointPtr, pointArray, 0, pointSize);
			Marshal.FreeHGlobal(pointPtr);

			var dataArray = Encoding.ASCII.GetBytes(data);

			var buffer = new List<byte>();
			buffer.AddRange(valueArray);
			buffer.AddRange(pointArray);
			buffer.AddRange(dataArray);

			return buffer.ToArray();
		}

		private static byte[] GenerateWithSpans(int value, Coordinate point, string data)
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

	public struct Coordinate
	{
		public int X;
		public int Y;
		public int Z;
	}
}