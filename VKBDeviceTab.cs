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
            Controls.Add(TLayout);
            TLayout.Controls.Add(TBox);
            TLayout.Controls.Add(FLayout);

        }
        public EncoderBox AddEncoderBox(VKBEncoder enc)
        {
            if (FLayout.InvokeRequired)
            {
                AddEncoderBoxCallback d = new AddEncoderBoxCallback(AddEncoderBox);
                return this.Invoke(d, new object[] { enc }) as EncoderBox;
            }
            EncoderBox box = new EncoderBox(enc);
            FLayout.Controls.Add(box);
            return box;
        }
    }
}
