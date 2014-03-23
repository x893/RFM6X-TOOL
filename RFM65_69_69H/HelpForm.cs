using SemtechLib.General.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SX1231SKB
{
	public class HelpForm : Form
	{
		private string docPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\Doc";
		private WebBrowser docViewer;

		public HelpForm()
		{
			InitializeComponent();
			if (!File.Exists(docPath + "\\overview.html"))
				return;
			docViewer.Navigate(docPath + "\\overview.html");
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(HelpForm));
			docViewer = new WebBrowser();
			SuspendLayout();
			docViewer.AllowWebBrowserDrop = false;
			docViewer.Dock = DockStyle.Fill;
			docViewer.IsWebBrowserContextMenuEnabled = false;
			docViewer.Location = new Point(0, 0);
			docViewer.MinimumSize = new Size(20, 18);
			docViewer.Name = "docViewer";
			docViewer.Size = new Size(292, 548);
			docViewer.TabIndex = 2;
			docViewer.TabStop = false;
			docViewer.Url = new Uri("", UriKind.Relative);
			docViewer.WebBrowserShortcutsEnabled = false;
			AutoScaleDimensions = new SizeF(6f, 12f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new Size(292, 548);
			Controls.Add((Control)docViewer);
			Icon = (Icon)resources.GetObject("$Icon");
			Name = "HelpForm";
			StartPosition = FormStartPosition.Manual;
			Text = "HelpForm";
			ResumeLayout(false);
		}

		public void UpdateDocument(DocumentationChangedEventArgs e)
		{
			string str = docPath + "\\" + e.DocFolder + "\\" + e.DocName + ".html";
			if (File.Exists(str))
			{
				docViewer.Navigate(str);
			}
			else
			{
				if (!File.Exists(docPath + "\\overview.html"))
					return;
				docViewer.Navigate(docPath + "\\overview.html");
			}
		}
	}
}