namespace SemtechLib.Ftdi
{
	public struct FtdiInfo
	{
		private uint deviceIndex;
		private uint flags;
		private string type;
		private uint id;
		private uint locId;
		private string serialNumber;
		private string description;

		public uint DeviceIndex
		{
			get { return deviceIndex; }
			set { deviceIndex = value; }
		}

		public uint Flags
		{
			get { return flags; }
			set { flags = value; }
		}

		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		public uint Id
		{
			get { return id; }
			set { id = value; }
		}

		public uint LocId
		{
			get { return locId; }
			set { locId = value; }
		}

		public string SerialNumber
		{
			get { return serialNumber; }
			set { serialNumber = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}
	}
}