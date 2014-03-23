using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SemtechLib.Controls.HexBoxCtrl
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public sealed class BuiltInContextMenu : Component
	{
		private HexBox _hexBox;
		private ContextMenuStrip _contextMenuStrip;
		private ToolStripMenuItem _cutToolStripMenuItem;
		private ToolStripMenuItem _copyToolStripMenuItem;
		private ToolStripMenuItem _pasteToolStripMenuItem;
		private ToolStripMenuItem _selectAllToolStripMenuItem;
		private string _copyMenuItemText;
		private string _cutMenuItemText;
		private string _pasteMenuItemText;
		private string _selectAllMenuItemText;
		private Image _cutMenuItemImage;
		private Image _copyMenuItemImage;
		private Image _pasteMenuItemImage;
		private Image _selectAllMenuItemImage;

		[DefaultValue(null)]
		[Localizable(true)]
		[Category("BuiltIn-ContextMenu")]
		public string CopyMenuItemText
		{
			get
			{
				return this._copyMenuItemText;
			}
			set
			{
				this._copyMenuItemText = value;
			}
		}

		[DefaultValue(null)]
		[Category("BuiltIn-ContextMenu")]
		[Localizable(true)]
		public string CutMenuItemText
		{
			get
			{
				return this._cutMenuItemText;
			}
			set
			{
				this._cutMenuItemText = value;
			}
		}

		[DefaultValue(null)]
		[Localizable(true)]
		[Category("BuiltIn-ContextMenu")]
		public string PasteMenuItemText
		{
			get
			{
				return this._pasteMenuItemText;
			}
			set
			{
				this._pasteMenuItemText = value;
			}
		}

		[Category("BuiltIn-ContextMenu")]
		[DefaultValue(null)]
		[Localizable(true)]
		public string SelectAllMenuItemText
		{
			get
			{
				return this._selectAllMenuItemText;
			}
			set
			{
				this._selectAllMenuItemText = value;
			}
		}

		internal string CutMenuItemTextInternal
		{
			get
			{
				if (string.IsNullOrEmpty(this.CutMenuItemText))
					return "Cut";
				else
					return this.CutMenuItemText;
			}
		}

		internal string CopyMenuItemTextInternal
		{
			get
			{
				if (string.IsNullOrEmpty(this.CopyMenuItemText))
					return "Copy";
				else
					return this.CopyMenuItemText;
			}
		}

		internal string PasteMenuItemTextInternal
		{
			get
			{
				if (string.IsNullOrEmpty(this.PasteMenuItemText))
					return "Paste";
				else
					return this.PasteMenuItemText;
			}
		}

		internal string SelectAllMenuItemTextInternal
		{
			get
			{
				if (string.IsNullOrEmpty(this.SelectAllMenuItemText))
					return "SelectAll";
				else
					return this.SelectAllMenuItemText;
			}
		}

		[Category("BuiltIn-ContextMenu")]
		[DefaultValue(null)]
		public Image CutMenuItemImage
		{
			get
			{
				return this._cutMenuItemImage;
			}
			set
			{
				this._cutMenuItemImage = value;
			}
		}

		[DefaultValue(null)]
		[Category("BuiltIn-ContextMenu")]
		public Image CopyMenuItemImage
		{
			get
			{
				return this._copyMenuItemImage;
			}
			set
			{
				this._copyMenuItemImage = value;
			}
		}

		[Category("BuiltIn-ContextMenu")]
		[DefaultValue(null)]
		public Image PasteMenuItemImage
		{
			get
			{
				return this._pasteMenuItemImage;
			}
			set
			{
				this._pasteMenuItemImage = value;
			}
		}

		[Category("BuiltIn-ContextMenu")]
		[DefaultValue(null)]
		public Image SelectAllMenuItemImage
		{
			get
			{
				return this._selectAllMenuItemImage;
			}
			set
			{
				this._selectAllMenuItemImage = value;
			}
		}

		internal BuiltInContextMenu(HexBox hexBox)
		{
			this._hexBox = hexBox;
			this._hexBox.ByteProviderChanged += new EventHandler(this.HexBox_ByteProviderChanged);
		}

		private void HexBox_ByteProviderChanged(object sender, EventArgs e)
		{
			this.CheckBuiltInContextMenu();
		}

		private void CheckBuiltInContextMenu()
		{
			if (Util.DesignMode)
				return;
			if (this._contextMenuStrip == null)
			{
				ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
				this._cutToolStripMenuItem = new ToolStripMenuItem(this.CutMenuItemTextInternal, this.CutMenuItemImage, new EventHandler(this.CutMenuItem_Click));
				contextMenuStrip.Items.Add((ToolStripItem)this._cutToolStripMenuItem);
				this._copyToolStripMenuItem = new ToolStripMenuItem(this.CopyMenuItemTextInternal, this.CopyMenuItemImage, new EventHandler(this.CopyMenuItem_Click));
				contextMenuStrip.Items.Add((ToolStripItem)this._copyToolStripMenuItem);
				this._pasteToolStripMenuItem = new ToolStripMenuItem(this.PasteMenuItemTextInternal, this.PasteMenuItemImage, new EventHandler(this.PasteMenuItem_Click));
				contextMenuStrip.Items.Add((ToolStripItem)this._pasteToolStripMenuItem);
				contextMenuStrip.Items.Add((ToolStripItem)new ToolStripSeparator());
				this._selectAllToolStripMenuItem = new ToolStripMenuItem(this.SelectAllMenuItemTextInternal, this.SelectAllMenuItemImage, new EventHandler(this.SelectAllMenuItem_Click));
				contextMenuStrip.Items.Add((ToolStripItem)this._selectAllToolStripMenuItem);
				contextMenuStrip.Opening += new CancelEventHandler(this.BuildInContextMenuStrip_Opening);
				this._contextMenuStrip = contextMenuStrip;
			}
			if (this._hexBox.ByteProvider == null && this._hexBox.ContextMenuStrip != null)
			{
				this._hexBox.ContextMenuStrip = (ContextMenuStrip)null;
			}
			else
			{
				if (this._hexBox.ByteProvider == null || this._hexBox.ContextMenuStrip != null)
					return;
				this._hexBox.ContextMenuStrip = this._contextMenuStrip;
			}
		}

		private void BuildInContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			this._cutToolStripMenuItem.Enabled = this._hexBox.CanCut();
			this._copyToolStripMenuItem.Enabled = this._hexBox.CanCopy();
			this._pasteToolStripMenuItem.Enabled = this._hexBox.CanPaste();
			this._selectAllToolStripMenuItem.Enabled = this._hexBox.CanSelectAll();
		}

		private void CutMenuItem_Click(object sender, EventArgs e)
		{
			this._hexBox.Copy();
		}

		private void CopyMenuItem_Click(object sender, EventArgs e)
		{
			this._hexBox.Copy();
		}

		private void PasteMenuItem_Click(object sender, EventArgs e)
		{
			this._hexBox.Copy();
		}

		private void SelectAllMenuItem_Click(object sender, EventArgs e)
		{
			this._hexBox.SelectAll();
		}
	}
}