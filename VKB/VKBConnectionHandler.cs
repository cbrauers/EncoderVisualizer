using HidSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncoderVisualizer.VKB
{
    internal class VKBConnectionHandler
    {
        public static VKBConnectionHandler Instance = new VKBConnectionHandler();
        private List<VKBDevice> Devices = new List<VKBDevice>();
        private VKBConnectionHandler() {
        }
        public void Startup(Object sender, EventArgs e)
        {
            IEnumerable<HidDevice> DevList = DeviceList.Local.GetHidDevices(vendorID: 0x231D);
            DeviceList.Local.Changed += DevicesChanged;
            foreach (HidDevice dev in DevList)
            {
                try
                {
                    if (dev.GetReportDescriptor().FeatureReports.Count() > 0)
                        Devices.Add(new VKBDevice(dev));
                }
                catch(NotSupportedException)
                {
                    // Ignore virtual controllers and the likes
                }
            }
        }
        public void DevicesChanged(Object sender, EventArgs e)
        {
            IEnumerable<HidDevice> DevList = DeviceList.Local.GetHidDevices(vendorID: 0x231D);
            List<VKBDevice> lostDevices = new List<VKBDevice>();
            foreach (VKBDevice dev in Devices)
            {
                if(!DevList.Contains(dev.HidDev)) lostDevices.Add(dev);
            }
            foreach (VKBDevice dev in lostDevices) {
                MainWindow.Instance.RemoveDevice(dev);
                Devices.Remove(dev);
            }
            foreach (HidDevice dev in DevList)
            {
                if(Devices.Find(d => d.HidDev == dev) == null)
                {
                    try
                    {
                        if (dev.GetReportDescriptor().FeatureReports.Count() > 0)
                            Devices.Add(new VKBDevice(dev));
                    }
                    catch (NotSupportedException)
                    {
                        // Ignore virtual controllers and the likes
                    }
                }
            }
        }
    }
}
