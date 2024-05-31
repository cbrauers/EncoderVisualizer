using EncoderVisualizer.VKB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncoderVisualizer
{
    delegate EncoderBox AddEncoderBoxCallback(VKBEncoder enc);
    public class VKBDeviceTab: TabPage
    {
        private readonly TableLayoutPanel TLayout;
        private readonly FlowLayoutPanel FLayout;
        private readonly Label TBox;
        private readonly Label NoEncs;
        private int encCount = 0;
        public VKBDeviceTab(VKBDevice dev) {
            TLayout = new TableLayoutPanel {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            FLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill
            };
            TBox = new Label
            {
                Dock = DockStyle.Top,
                Text = $"{dev.DeviceName}, PID {dev.HidDev.ProductID:X4}, S/N {dev.SerialNumber}"
            };
            NoEncs = new Label
            {
                Dock = DockStyle.Fill,
                Text = "No Encoders detected. Make sure that the device:\n" +
                "1. is running nJoy32 firmware 2.17.9 or newer\n" +
                "2. has \"Virtual BUS over USB\" enabled in VKBDevCfg (and the setting has been Set to the device)\n" +
                "3. has encoders configured on the physical button layer",
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 16, System.Drawing.FontStyle.Bold)
            };

            Controls.Add(TLayout);
            TLayout.Controls.Add(TBox);
            TLayout.Controls.Add(NoEncs);

        }
        public EncoderBox AddEncoderBox(VKBEncoder enc)
        {
            if (FLayout.InvokeRequired || TLayout.InvokeRequired)
            {
                AddEncoderBoxCallback d = new AddEncoderBoxCallback(AddEncoderBox);
                return this.Invoke(d, new object[] { enc }) as EncoderBox;
            }
            if(encCount == 0)
            {
                TLayout.Controls.Remove(NoEncs);
                TLayout.Controls.Add(FLayout);
            }
            encCount++;
            EncoderBox box = new EncoderBox(enc);
            FLayout.Controls.Add(box);
            return box;
        }
    }
}
