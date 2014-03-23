namespace SemtechLib.Ftdi
{
	public class MpsseSPI : Mpsse
	{
		public MpsseSPI(string device)
			: base(device)
		{
			Device = device;
			portDir = (byte)251;
			portValue = (byte)30;
		}

		public override void ScanInOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst)
		{
			int num1 = bitCount / 8;
			if (num1 > 0)
			{
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add((byte)53);
				else
					txBuffer.Add((byte)61);
				txBuffer.Add((byte)(num1 - 1 & (int)byte.MaxValue));
				txBuffer.Add((byte)(num1 - 1 >> 8 & (int)byte.MaxValue));
				for (int index = 0; index < num1; ++index)
					txBuffer.Add(data[index]);
			}
			int num2 = bitCount % 8;
			if (num2 <= 0)
				return;
			if (clockOutDataBitsMSBFirst)
				txBuffer.Add((byte)55);
			else
				txBuffer.Add((byte)63);
			txBuffer.Add((byte)(num2 - 1 & (int)byte.MaxValue));
			txBuffer.Add(data[data.Length - 1]);
		}

		public override void ScanIn(int bitCount, bool clockOutDataBitsMSBFirst)
		{
			int num1 = bitCount / 8;
			if (num1 > 0)
			{
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add((byte)36);
				else
					txBuffer.Add((byte)44);
				txBuffer.Add((byte)(num1 - 1 & (int)byte.MaxValue));
				txBuffer.Add((byte)(num1 - 1 >> 8 & (int)byte.MaxValue));
			}
			int num2 = bitCount % 8;
			if (num2 <= 0)
				return;
			if (clockOutDataBitsMSBFirst)
				txBuffer.Add((byte)38);
			else
				txBuffer.Add((byte)46);
			txBuffer.Add((byte)(num2 - 1 & (int)byte.MaxValue));
		}

		public override void ScanOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst)
		{
			int num1 = bitCount / 8;
			if (num1 > 0)
			{
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add((byte)17);
				else
					txBuffer.Add((byte)25);
				txBuffer.Add((byte)(num1 - 1 & (int)byte.MaxValue));
				txBuffer.Add((byte)(num1 - 1 >> 8 & (int)byte.MaxValue));
				for (int index = 0; index < num1; ++index)
					txBuffer.Add(data[index]);
			}
			int num2 = bitCount % 8;
			if (num2 <= 0)
				return;
			if (clockOutDataBitsMSBFirst)
				txBuffer.Add((byte)19);
			else
				txBuffer.Add((byte)27);
			txBuffer.Add((byte)(num2 - 1 & (int)byte.MaxValue));
			txBuffer.Add(data[data.Length - 1]);
		}
	}
}