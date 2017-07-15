using System;

namespace ClassesAndStructs
{
	public struct TonsOfDataAsStruct
	{
		private readonly int[] data;

		public TonsOfDataAsStruct(int size) => this.data = new int[size];

		public void ChangeData()
		{
			var random = new Random();
			var index = random.Next(this.data.Length);
			this.data[index] = random.Next();
		}
	}
}
