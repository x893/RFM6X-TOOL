using System;
using System.Collections;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml;

namespace SemtechLib.General
{
	public sealed class ApplicationSettings : IDisposable
	{
		private const string FileName = "ApplicationSettings.xml";
		private const string RootElement = "ApplicationSettings";
		private const string SettingElement = "Setting";
		private const string PathSeperator = "/";
		private XmlDocument Document;

		public XmlDocument XmlDocument
		{
			get { return Document; }
		}

		public ApplicationSettings()
		{
			Document = ApplicationSettings.OpenDocument();
		}

		public bool SetValue(string Name, string Value)
		{
			foreach (XmlNode xmlNode in Document.SelectNodes("/ApplicationSettings/Setting"))
			{
				if (xmlNode.Attributes["Name"].Value.Equals(Name))
				{
					xmlNode.Attributes["Value"].Value = Value;
					return false;
				}
			}
			XmlNode xmlNode1 = Document.SelectSingleNode("/ApplicationSettings");
			XmlNode newChild = (XmlNode)Document.CreateElement("Setting");
			newChild.Attributes.Append(Document.CreateAttribute("Name"));
			newChild.Attributes.Append(Document.CreateAttribute("Value"));
			newChild.Attributes["Name"].Value = Name;
			newChild.Attributes["Value"].Value = Value;
			xmlNode1.AppendChild(newChild);
			return true;
		}

		public bool RemoveValue(string Name)
		{
			foreach (XmlNode oldChild in Document.SelectNodes("/ApplicationSettings/Setting"))
			{
				if (oldChild.Attributes["Name"].Value.Equals(Name))
				{
					oldChild.ParentNode.RemoveChild(oldChild);
					return true;
				}
			}
			return false;
		}

		public string GetValue(string Name)
		{
			foreach (XmlNode xmlNode in Document.SelectNodes("/ApplicationSettings/Setting"))
			{
				if (xmlNode.Attributes["Name"].Value.Equals(Name))
					return xmlNode.Attributes["Value"].Value;
			}
			return (string)null;
		}

		public void ClearSettings()
		{
			Document = ApplicationSettings.CreateDocument();
		}

		public Hashtable GetSettings()
		{
			XmlNodeList xmlNodeList = Document.SelectNodes("/ApplicationSettings/Setting");
			Hashtable hashtable = new Hashtable(xmlNodeList.Count);
			foreach (XmlNode xmlNode in xmlNodeList)
				hashtable.Add((object)xmlNode.Attributes["Name"].Value, (object)xmlNode.Attributes["Value"].Value);
			return hashtable;
		}

		public void SaveConfiguration()
		{
			ApplicationSettings.SaveDocument(Document, "ApplicationSettings.xml");
		}

		private static XmlDocument OpenDocument()
		{
			IsolatedStorageFileStream storageFileStream;
			try
			{
				storageFileStream = new IsolatedStorageFileStream("ApplicationSettings.xml", FileMode.Open, FileAccess.Read);
			}
			catch (FileNotFoundException)
			{
				return ApplicationSettings.CreateDocument();
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlTextReader xmlTextReader = new XmlTextReader((Stream)storageFileStream);
			xmlDocument.Load((XmlReader)xmlTextReader);
			xmlTextReader.Close();
			storageFileStream.Close();
			return xmlDocument;
		}

		private static XmlDocument CreateDocument()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.CreateXmlDeclaration("1.0", (string)null, "yes");
			XmlElement element = xmlDocument.CreateElement("ApplicationSettings");
			xmlDocument.AppendChild((XmlNode)element);
			return xmlDocument;
		}

		private static void SaveDocument(XmlDocument document, string filename)
		{
			IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
			storageFileStream.SetLength(0L);
			XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream)storageFileStream, (Encoding)new UnicodeEncoding());
			xmlTextWriter.Formatting = Formatting.Indented;
			document.Save((XmlWriter)xmlTextWriter);
			xmlTextWriter.Close();
			storageFileStream.Close();
		}

		public void Dispose()
		{
			ApplicationSettings.SaveDocument(Document, "ApplicationSettings.xml");
		}
	}
}