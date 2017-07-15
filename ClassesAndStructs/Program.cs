using System;

namespace ClassesAndStructs
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}

		private static TonsOfDataAsClass GenerateDataAsClass(int size)
		{
			var data = new TonsOfDataAsClass(size);

			for(var i = 0; i < size; i++)
			{
				data.ChangeData();
			}

			return data;
		}

		private static TonsOfDataAsStruct GenerateDataAsStruct(int size)
		{
			var data = new TonsOfDataAsStruct(size);

			for (var i = 0; i < size; i++)
			{
				data.ChangeData();
			}

			return data;
		}
	}
}