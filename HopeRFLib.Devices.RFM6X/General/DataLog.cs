using SemtechLib.Devices.SX1231;
using SemtechLib.General.Events;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace SemtechLib.Devices.SX1231.General
{
	public class DataLog : INotifyPropertyChanged
	{
		private string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		private string fileName = "sx1231-Rssi.log";
		private ulong maxSamples = 1000UL;
		private CultureInfo ci = CultureInfo.InvariantCulture;
		private FileStream fileStream;
		private StreamWriter streamWriter;
		private bool state;
		private ulong samples;
		private SX1231 sx1231;

		public SX1231 SX1231
		{
			set
			{
				if (sx1231 == value)
					return;
				sx1231 = value;
				sx1231.PropertyChanged += new PropertyChangedEventHandler(sx1231_PropertyChanged);
			}
		}

		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				path = value;
				OnPropertyChanged("Path");
			}
		}

		public string FileName
		{
			get
			{
				return fileName;
			}
			set
			{
				fileName = value;
				OnPropertyChanged("FileName");
			}
		}

		public ulong MaxSamples
		{
			get
			{
				return maxSamples;
			}
			set
			{
				maxSamples = value;
				OnPropertyChanged("MaxSamples");
			}
		}

		public event ProgressEventHandler ProgressChanged;

		public event EventHandler Stoped;

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnProgressChanged(ulong progress)
		{
			if (ProgressChanged == null)
				return;
			ProgressChanged((object)this, new ProgressEventArg(progress));
		}

		private void OnStop()
		{
			if (Stoped == null)
				return;
			Stoped((object)this, EventArgs.Empty);
		}

		private void GenerateFileHeader()
		{
			string str = sx1231.RfPaSwitchEnabled == 0 ? "#\tTime\tRSSI" : "#\tTime\tRF_PA RSSI\tRF_IO RSSI";
			streamWriter.WriteLine("#\tSX1231 data log generated the " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString());
			streamWriter.WriteLine(str);
		}

		private void Update()
		{
			string str1 = "\t";
			if (sx1231 == null || !state)
				return;
			if (samples < maxSamples || (long)maxSamples == 0L)
			{
				string str2;
				if (sx1231.RfPaSwitchEnabled != 0)
					str2 = str1 + DateTime.Now.ToString("HH:mm:ss.fff", (IFormatProvider)ci) + "\t" + sx1231.RfPaRssiValue.ToString("F1") + "\t" + sx1231.RfIoRssiValue.ToString("F1");
				else
					str2 = str1 + DateTime.Now.ToString("HH:mm:ss.fff", (IFormatProvider)ci) + "\t" + sx1231.RssiValue.ToString("F1");
				streamWriter.WriteLine(str2);
				if ((long)maxSamples != 0L)
				{
					++samples;
					OnProgressChanged((ulong)((Decimal)samples * new Decimal(100) / (Decimal)maxSamples));
				}
				else
					OnProgressChanged(0UL);
			}
			else
				OnStop();
		}

		public void Start()
		{
			try
			{
				fileStream = new FileStream(path + "\\" + fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
				streamWriter = new StreamWriter((Stream)fileStream, Encoding.ASCII);
				GenerateFileHeader();
				samples = 0UL;
				state = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Stop()
		{
			try
			{
				state = false;
				streamWriter.Close();
			}
			catch (Exception)
			{
			}
		}

		private void sx1231_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sx1231.RfPaSwitchEnabled != 0)
			{
				switch (e.PropertyName)
				{
					case "RfPaRssiValue":
						if (sx1231.RfPaSwitchEnabled != 1)
							break;
						Update();
						break;
					case "RfIoRssiValue":
						Update();
						break;
				}
			}
			else
			{
				switch (e.PropertyName)
				{
					case "RssiValue":
						Update();
						break;
				}
			}
		}

		private void OnPropertyChanged(string propName)
		{
			if (PropertyChanged == null)
				return;
			PropertyChanged((object)this, new PropertyChangedEventArgs(propName));
		}
	}
}