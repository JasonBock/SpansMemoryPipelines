namespace PlayingWithMemory
{
	public sealed class RefReturns
	{
		private readonly byte[] data = new byte[1000];
		//private readonly List<byte> data = new List<byte>();

		public ref byte GetIndividualData(int index) =>
			ref this.data[index];
	}
}
