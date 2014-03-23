using System;

namespace SemtechLib.Ftdi
{
	public class MpsseI2C : Mpsse
	{
		public MpsseI2C(string device)
			: base(device)
		{
			Device = device;
			portDir = (byte)0;
			portValue = (byte)0;
		}

		public override void ScanInOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst)
		{
			throw new NotImplementedException();
		}

		public override void ScanIn(int bitCount, bool clockOutDataBitsMSBFirst)
		{
			if (bitCount - 1 == 0)
			{
				txBuffer.Add((byte)39);
				txBuffer.Add((byte)0);
			}
			else
			{
				int num1 = bitCount / 8;
				if (num1 > 0)
				{
					txBuffer.Add((byte)37);
					txBuffer.Add((byte)(num1 - 1 & (int)byte.MaxValue));
					txBuffer.Add((byte)(num1 - 1 >> 8 & (int)byte.MaxValue));
				}
				int num2 = bitCount % 8;
				if (num2 <= 0)
					return;
				txBuffer.Add((byte)39);
				txBuffer.Add((byte)(num2 - 1 & (int)byte.MaxValue));
			}
		}

		public override void ScanOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst)
		{
			int num1 = bitCount / 8;
			if (num1 > 0)
			{
				txBuffer.Add((byte)17);
				txBuffer.Add((byte)(num1 - 1 & (int)byte.MaxValue));
				txBuffer.Add((byte)(num1 - 1 >> 8 & (int)byte.MaxValue));
				for (int index = 0; index < num1; ++index)
					txBuffer.Add(data[index]);
			}
			int num2 = bitCount % 8;
			if (num2 <= 0)
				return;
			txBuffer.Add((byte)19);
			txBuffer.Add((byte)(num2 - 1 & (int)byte.MaxValue));
			txBuffer.Add(data[data.Length - 1]);
		}
	}
}