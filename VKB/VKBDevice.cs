using HidSharp;
using HidSharp.Reports;
using HidSharp.Reports.Input;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EncoderVisualizer.VKB
{
    public class VKBDevice
    {
        public string DeviceName = "";
        public string SerialNumber = "";
        public HidDevice HidDev;
        private HidStream Stream;
        private HidDeviceInputReceiver Receiver;
        public VKBDeviceTab Tab;
        private SortedList<byte, VKBEncoder> Encoders = new SortedList<byte, VKBEncoder>();
        private byte lastSeqNo;
        public VKBDevice(HidDevice dev) {
            HidDev = dev;
            DeviceName = dev.GetProductName().Trim();

            SerialNumber = "None";
            try
            {
                SerialNumber = dev.GetSerialNumber();
            }
            catch (IOException) { }
            Tab = MainWindow.Instance.AddDevice(this);
            Stream = dev.Open();
            Stream.ReadTimeout = System.Threading.Timeout.Infinite;
            ReportDescriptor descriptor = new ReportDescriptor(VKBHidReport.Descriptor);
            HidDeviceInputReceiver Receiver = new HidDeviceInputReceiver(descriptor);
            Receiver.Received += OnHidReportReceived;
            Receiver.Start(Stream);
        }
        private void OnHidReportReceived(object sender, System.EventArgs e)
        {
            byte[] InputReportBuffer = new byte[64];
            var inputReceiver = sender as HidDeviceInputReceiver;
            while (inputReceiver.TryRead(InputReportBuffer, 0, out _))
            {
                byte ReportId = InputReportBuffer[0];
                if (ReportId != 0x08) // 0x08 = Monitoring channel / virtual bus
                    continue;
                byte MessageType = InputReportBuffer[1];
                if (MessageType != 0x13) // 0x13 = Encoder status
                    continue;
                ParseEncoderReport(InputReportBuffer);
            }
        }
        private void ParseEncoderReport(byte[] Report)
        {
            byte sequenceNo = Report[2];
            lastSeqNo = sequenceNo;
            byte encoderCount = Report[3];
            int maxEncoders = (Report.Length - 4) / 2;
            if (encoderCount > maxEncoders)
            {
                encoderCount = (byte)maxEncoders;
            }
            for (byte i = 0; i < encoderCount; i++)
            {
                ushort newPos = (ushort)(Report[5 + 2 * i] << 8 | Report[4 + 2 * i]);
                if (!Encoders.ContainsKey(i))
                {
                    Encoders.Add(i, new VKBEncoder(this,i));
                }
                else
                {
                    Encoders[i].Update(newPos);
                }
            }
        }
        public EncoderBox AddEncoderBox(VKBEncoder enc)
        {
            return Tab.AddEncoderBox(enc);
        }
    }
}
