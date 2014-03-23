namespace SemtechLib.General
{
	public class BindingRegister : EditableObject
	{
		private string m_name;
		private uint m_address;
		private uint m_value;
		private bool m_readOnly;

		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		public uint Address
		{
			get { return m_address; }
			set { m_address = value; }
		}

		public uint Value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		public bool ReadOnly
		{
			get { return m_readOnly; }
			set { m_readOnly = value; }
		}

		public BindingRegister()
		{
			m_name = "";
			m_address = 0U;
			m_value = 0U;
		}

		public BindingRegister(string name, uint address, uint value)
		{
			m_name = name;
			m_address = address;
			m_value = value;
			m_readOnly = false;
		}

		public BindingRegister(string name, uint address, uint value, bool readOnly)
			: this(name, address, value)
		{
			m_readOnly = readOnly;
		}
	}
}