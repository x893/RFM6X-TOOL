using System.Text;

namespace SemtechLib.Controls.HexBoxCtrl
{
	public class EbcdicByteCharProvider : IByteCharConverter
	{
		private Encoding _ebcdicEncoding = Encoding.GetEncoding(500);

		public char ToChar(byte b)
		{
			string @string = this._ebcdicEncoding.GetString(new byte[1]
      {
        b
      });
			if (@string.Length <= 0)
				return '.';
			else
				return @string[0];
		}

		public byte ToByte(char c)
		{
			byte[] bytes = this._ebcdicEncoding.GetBytes(new char[1]
      {
        c
      });
			if (bytes.Length <= 0)
				return (byte)0;
			else
				return bytes[0];
		}

		public override string ToString()
		{
			return "EBCDIC (Code Page 500)";
		}
	}
}