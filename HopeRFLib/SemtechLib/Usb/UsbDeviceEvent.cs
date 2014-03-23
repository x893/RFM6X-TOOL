using System;
using System.Management;

namespace SemtechLib.Usb
{
	internal class UsbDeviceEvent
	{
		private string m_deviceId = "";
		private ManagementEventWatcher creationEventWatcher;
		private ManagementEventWatcher deletionEventWatcher;

		public event EventHandler Attached;
		public event EventHandler Detached;

		public UsbDeviceEvent()
		{
			creationEventWatcher = (ManagementEventWatcher)null;
			ManagementOperationObserver operationObserver = new ManagementOperationObserver();
			ManagementScope scope = new ManagementScope("root\\CIMV2");
			scope.Options.EnablePrivileges = true;
			try
			{
				WqlEventQuery wqlEventQuery = new WqlEventQuery();
				wqlEventQuery.EventClassName = "__InstanceCreationEvent";
				wqlEventQuery.WithinInterval = new TimeSpan(0, 0, 3);
				wqlEventQuery.Condition = "TargetInstance ISA 'Win32_USBControllerDevice'";
				Console.WriteLine(wqlEventQuery.QueryString);
				creationEventWatcher = new ManagementEventWatcher(scope, (EventQuery)wqlEventQuery);
				creationEventWatcher.EventArrived += new EventArrivedEventHandler(creationEventWatcher_EventArrived);
				creationEventWatcher.Start();
				wqlEventQuery.EventClassName = "__InstanceDeletionEvent";
				wqlEventQuery.WithinInterval = new TimeSpan(0, 0, 3);
				wqlEventQuery.Condition = "TargetInstance ISA 'Win32_USBControllerdevice'";
				Console.WriteLine(wqlEventQuery.QueryString);
				deletionEventWatcher = new ManagementEventWatcher(scope, (EventQuery)wqlEventQuery);
				deletionEventWatcher.EventArrived += new EventArrivedEventHandler(deletionEventWatcher_EventArrived);
				deletionEventWatcher.Start();
			}
			catch
			{
				Dispose();
			}
		}

		public UsbDeviceEvent(string deviceId)
			: this()
		{
			m_deviceId = deviceId;
		}

		private void OnAttached()
		{
			if (Attached == null)
				return;
			Attached((object)this, EventArgs.Empty);
		}

		private void OnDetached()
		{
			if (Detached == null)
				return;
			Detached((object)this, EventArgs.Empty);
		}

		public void Dispose()
		{
			if (creationEventWatcher != null)
				creationEventWatcher.Stop();
			if (deletionEventWatcher != null)
				deletionEventWatcher.Stop();
			creationEventWatcher = (ManagementEventWatcher)null;
			deletionEventWatcher = (ManagementEventWatcher)null;
		}

		private void creationEventWatcher_EventArrived(object sender, EventArrivedEventArgs e)
		{
			foreach (PropertyData propertyData1 in e.NewEvent.Properties)
			{
				ManagementBaseObject managementBaseObject;
				if ((managementBaseObject = propertyData1.Value as ManagementBaseObject) != null)
				{
					foreach (PropertyData propertyData2 in managementBaseObject.Properties)
					{
						string str = propertyData2.Value as string;
						if (str != null && str.Replace("\\", "").Contains(m_deviceId.Replace("\\", "")))
							OnAttached();
					}
				}
			}
		}

		private void deletionEventWatcher_EventArrived(object sender, EventArrivedEventArgs e)
		{
			foreach (PropertyData propertyData1 in e.NewEvent.Properties)
			{
				ManagementBaseObject managementBaseObject;
				if ((managementBaseObject = propertyData1.Value as ManagementBaseObject) != null)
				{
					foreach (PropertyData propertyData2 in managementBaseObject.Properties)
					{
						string str = propertyData2.Value as string;
						if (str != null && str.Replace("\\", "").Contains(m_deviceId.Replace("\\", "")))
							OnDetached();
					}
				}
			}
		}
	}
}