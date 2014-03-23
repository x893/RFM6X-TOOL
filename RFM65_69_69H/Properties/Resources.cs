using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace SX1231SKB.Properties
{
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;
		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals((object)Resources.resourceMan, (object)null))
					Resources.resourceMan = new ResourceManager("SX1231SKB.Properties.Resources", typeof(Resources).Assembly);
				return Resources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		internal static Bitmap sx1231_pr
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("sx1231_pr", Resources.resourceCulture);
			}
		}

		internal static Bitmap sx1231_pr1
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("sx1231_pr1", Resources.resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}