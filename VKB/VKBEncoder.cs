using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncoderVisualizer.VKB
{
    public class VKBEncoder
    {
        public ushort? Value = null;
        public byte Id;
        public VKBDevice ParentDevice = null;
        private readonly EncoderBox Box;
        public VKBEncoder(VKBDevice dev, byte id)
        {
            ParentDevice = dev;
            Id = id;
            Box = dev.AddEncoderBox(this);
            Update(0);
        }
        public void Update(ushort value)
        {
            bool firstdraw = false;
            if (Value == null)
            {
                Value = 0;
                firstdraw = true;
            }
            short delta = (short)((value - Value) & 0xFFFF);
            if (delta == 0 && !firstdraw) return;
            Box.UpdateAngle(12 * delta);
            Value = value;
            Box.UpdateLabel($"{Value:X4}");
        }
    }
}
