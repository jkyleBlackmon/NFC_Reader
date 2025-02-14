using Sydesoft.NfcDevice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace acr122_demo
{
    public partial class RetroLauncher : Form
    {
        private static MyACR122U acr122u = new MyACR122U();

        //[System.Runtime.InteropServices.DllImport("User32.dll")]
        //private static extern bool SetForegroundWindow(IntPtr handle);

        private IntPtr handle;

        static List<string> ids = new List<string>();
        static List<string> emus = new List<string>();
        static List<string> cores = new List<string>();
        static List<string> roms = new List<string>();


        static Process retroarchProcess;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public RetroLauncher()
        {
            InitializeComponent();
            try
            {
                acr122u.Init(false, 50, 4, 4, 200);  // NTAG213
                acr122u.CardInserted += Acr122u_CardInserted;
                acr122u.CardRemoved += Acr122u_CardRemoved;
            }
            catch (PCSC.Exceptions.NoServiceException)
            {
                MessageBox.Show(this, "Failed to find a reader connected to the system", "No reader connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (File.Exists("mapping.txt"))
                using (var reader = new StreamReader("mapping.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        ids.Add(values[0]);
                        emus.Add(values[1]);
                        cores.Add(values[2]);
                        roms.Add(values[3]);
                    }
                }
            else
            {
                MessageBox.Show(this, "No mapping.txt found. Please create a mapping.txt in the format:\n[ID],[Path To Emulator],[Path To Core],[Path To Rom]", "No mapping.txt found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = 0; i < cores.Count; i++)
            {
                Console.WriteLine(ids[i] + cores[i] + roms[i]);
            }

        }
        private static void Acr122u_CardInserted(PCSC.ICardReader reader)
        {
            Console.WriteLine("NFC tag placed on reader.");
            acr122u.ReadId = BitConverter.ToString(acr122u.GetUID(reader)).Replace("-", "");
            Console.WriteLine("Unique ID: " + acr122u.ReadId);
            LoadRom(acr122u.ReadId);


        }

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static void LoadRom(string id)
        {



            if (ids.Contains(id))
            {
                Console.WriteLine("Loading  " + roms[ids.IndexOf(id)]);

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = GetEmuName(id);
                startInfo.Arguments = "-f -L \"" + GetCoreName(id) + "\" \"" + GetRomName(id) + "\"";
                retroarchProcess = Process.Start(startInfo);
                System.Threading.Thread.Sleep(2000);
                // ShowWindow(retroarchProcess.MainWindowHandle, 9);
                // System.Threading.Thread.Sleep(100);
                // SetForegroundWindow(retroarchProcess.MainWindowHandle);
                // System.Threading.Thread.Sleep(100);
                // SwitchToThisWindow(retroarchProcess.MainWindowHandle, true);

                SetCursorPos(5, 5);
                uint X = (uint)Cursor.Position.X;
                uint Y = (uint)Cursor.Position.Y;

                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);

            }
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        //[DllImport("User32.dll", SetLastError = true)]
        //static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        private static string GetEmuName(string id)
        {
            if (ids.Contains(id))
            {
                return emus[ids.IndexOf(id)];
            }
            else
                return "";
        }
        private static string GetRomName(string id)
        {
            if (ids.Contains(id))
            {
                return roms[ids.IndexOf(id)];
            }
            else
                return "";
        }
        private static string GetCoreName(string id)
        {
            if (ids.Contains(id))
            {
                return cores[ids.IndexOf(id)];
            }
            else
                return "";
        }

        private static void Acr122u_CardRemoved()
        {
            Console.WriteLine("NFC tag removed.");
            if (retroarchProcess != null)
                try { retroarchProcess.Kill(); } catch (Exception ex) { Console.WriteLine(ex.Message); }
            ;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = acr122u.ReadId;
            textBox2.Text = GetCoreName(acr122u.ReadId);
            textBox3.Text = GetRomName(acr122u.ReadId); ;
            textBox4.Text = GetEmuName(acr122u.ReadId); ;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    internal class MyACR122U : ACR122U
    {
        private string readId;
        public string ReadId
        {
            get { return readId; }
            set { readId = value; }
        }

        public MyACR122U()
        {

        }
    }
}
