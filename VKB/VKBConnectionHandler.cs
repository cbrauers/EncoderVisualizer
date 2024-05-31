﻿using HidSharp;
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
            foreach (HidDevice dev in DevList)
            {
                Devices.Add(new VKBDevice(dev));
            }
        }
    }
}