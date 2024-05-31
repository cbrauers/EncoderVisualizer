using EncoderVisualizer.VKB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncoderVisualizer
{
    public class EncoderBox: Panel
    {
        delegate void AngleUpdateCallback(float ang);
        delegate void LabelUpdateCallback(string text);
        private VKBEncoder Enc;
        private Bitmap EncoderImage = new Bitmap(Properties.Resources.knob);
        private Label NumberLabel;
        private Label ValueLabel;
        private float angle = 0.0f;
        public EncoderBox(VKBEncoder encoder) {
            Size = new Size(96, 128);
            Enc = encoder;
            BorderStyle = BorderStyle.Fixed3D;
            Paint += new PaintEventHandler(HandlePaint);
            NumberLabel = new Label
            {
                Text = $"Encoder {Enc.Id + 1}",
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 0),
                Size = new Size(96, 32)
            };
            Controls.Add(NumberLabel);
            ValueLabel = new Label
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 96),
                Size = new Size(96, 32)
            };
            Controls.Add(ValueLabel);
        }
        public void UpdateAngle(float ang)
        {
            if (InvokeRequired)
            {
                AngleUpdateCallback d = new AngleUpdateCallback(UpdateAngle);
                Invoke(d, new object[] { ang });
                return;
            }
            angle += ang;
            Refresh();
        }
        public void UpdateLabel(string text)
        {
            if (InvokeRequired)
            {
                LabelUpdateCallback d = new LabelUpdateCallback(UpdateLabel);
                Invoke(d, new object[] { text });
                return;
            }
            ValueLabel.Text = text;
            ValueLabel.Refresh();
        }
        private void HandlePaint(object sender, PaintEventArgs e)
        {
            Bitmap encoderrotated = new Bitmap (EncoderImage.Width, EncoderImage.Height);
            using (Graphics g = Graphics.FromImage(encoderrotated)) {
                g.TranslateTransform((float)encoderrotated.Width / 2, (float)encoderrotated.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)encoderrotated.Width / 2, -(float)encoderrotated.Height / 2);
                g.DrawImage(EncoderImage, 0, 0);
            }
            using (Graphics g = e.Graphics)
            {
                g.DrawImage(encoderrotated, 16.0f, 32.0f, 64.0f, 64.0f);
            }
        }
    }
}
