using Sydesoft.NfcDevice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace acr122_demo {
    public partial class Form1 : Form {
        private static MyACR122U acr122u = new MyACR122U();
        public Form1() {
            InitializeComponent();
            try {
              acr122u.Init(false, 50, 4, 4, 200);  // NTAG213
                acr122u.CardInserted += Acr122u_CardInserted;
                acr122u.CardRemoved += Acr122u_CardRemoved;
            }
            catch (ArgumentException) {
                MessageBox.Show(this, "Failed to find a reader connected to the system", "No reader connected", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private static void Acr122u_CardInserted(PCSC.ICardReader reader) {
            Console.WriteLine("NFC tag placed on reader.");
            acr122u.ReadId = BitConverter.ToString(acr122u.GetUID(reader)).Replace("-", "");
            Console.WriteLine("Unique ID: " + acr122u.ReadId);
            string data = "Hello World";
            Console.WriteLine("Write data to NFC tag: " + data);
            bool ret = acr122u.WriteData(reader, Encoding.UTF8.GetBytes(data));
            Console.WriteLine("Write result: " + (ret ? "Success" : "Failed"));
            Console.WriteLine("Read data from tag: " + Encoding.UTF8.GetString(acr122u.ReadData(reader)));
        }

        private static void Acr122u_CardRemoved() {
            Console.WriteLine("NFC tag removed.");
        }

        private void timer1_Tick(object sender, EventArgs e) {
            textBox1.Text = acr122u.ReadId;
        }
    }

    internal class MyACR122U : ACR122U {
        private string readId;
        public string ReadId {
            get { return readId; }
            set { readId = value; }
        }

        public MyACR122U() {
            
        }
    }
}
