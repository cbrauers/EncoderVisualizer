using EncoderVisualizer.VKB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace EncoderVisualizer
{
    public class MainWindow: Form
    {
        delegate VKBDeviceTab AddTabCallback(VKBDevice dev);
        delegate void RemoveTabCallback(VKBDevice dev);
        public static MainWindow Instance { get { return actualInstance ?? (actualInstance = new MainWindow()); } }
        private static MainWindow actualInstance = null;
        private readonly TabControl Devices;
        private MainWindow() {
            Size = new System.Drawing.Size(640, 480);
            Text = "Encoder Visualizer for VKB Devices";
            Devices = new TabControl
            {
                Dock = DockStyle.Fill,
                Multiline = true
            };
            Controls.Add(Devices);
            this.Shown += new EventHandler(VKBConnectionHandler.Instance.Startup);
            Icon = Properties.Resources.Encoder;
        }
        public VKBDeviceTab AddDevice(VKBDevice dev)
        {
            if (InvokeRequired)
            {
                AddTabCallback d = new AddTabCallback(AddDevice);
                return Invoke(d, new object[] { dev }) as VKBDeviceTab;
            }
            VKBDeviceTab tab = new VKBDeviceTab(dev);
            tab.Text = dev.DeviceName;
            Devices.Controls.Add(tab);
            return tab;
        }
        public void RemoveDevice(VKBDevice dev)
        {
            if (InvokeRequired)
            {
                RemoveTabCallback d = new RemoveTabCallback(RemoveDevice);
                Invoke(d, new object[] { dev });
                return;
            }
            Devices.Controls.Remove(dev.Tab);
        }

    }
}
