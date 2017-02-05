using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using ThoughtWorks.QRCode.Codec;

namespace PrintItNow
{
    public partial class Form1 : Form
    {
        PrintDocument pdo = new PrintDocument();

        QRCodeEncoder qrce = new QRCodeEncoder();
        Bitmap qrcode;

        public Form1()
        {
            InitializeComponent();
            //printer init
            pdo.PrintPage += new PrintPageEventHandler(onPrintPage);
            

            //qrcode init
            qrce.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrce.QRCodeScale = 1;
            qrce.QRCodeVersion = 7;
            qrce.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
        }

        private void onPrintPage(object sender, PrintPageEventArgs e)
        {
            // write qrcode
            qrcode = CreateImage(qrce.Encode(textBox10.Text));
            e.Graphics.DrawImage(qrcode, int.Parse(textBox12.Text), int.Parse(textBox13.Text));

            // perpare strings
            string[] lines = new string[3];
            lines[0] = textBox1.Text;
            lines[1] = textBox2.Text;
            lines[2] = textBox3.Text;

            // position set
            int x = int.Parse(textBox7.Text);
            int y = int.Parse(textBox8.Text);

            // write
            foreach (string line in lines)
            {
                e.Graphics.DrawString(line, new Font("Arial", 10), Brushes.Black, x, y);
                y += int.Parse(textBox9.Text);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pdo.DefaultPageSettings.PaperSize = new PaperSize("tag", int.Parse(textBox4.Text), int.Parse(textBox5.Text));
            if (textBox6.Text == "Landscape")
            {
                pdo.DefaultPageSettings.Landscape = true;
            }else
            {
                pdo.DefaultPageSettings.Landscape = false;
            }
            
            pdo.Print();
        }







        public Bitmap CreateImage(bool[][] matrix)
        {
            int qrCodeScale = int.Parse(textBox11.Text);
            SolidBrush brush = new SolidBrush(Color.White);
            Bitmap image = new Bitmap((matrix.Length * qrCodeScale) + 1, (matrix.Length * qrCodeScale) + 1);
            Graphics g = Graphics.FromImage(image);
            g.FillRectangle(brush, new Rectangle(0, 0, image.Width, image.Height));
            brush.Color = Color.Black;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (matrix[j][i])
                    {
                        g.FillRectangle(brush, j * qrCodeScale, i * qrCodeScale, qrCodeScale, qrCodeScale);
                    }
                }
            }
            return image;
        }
    }
}
