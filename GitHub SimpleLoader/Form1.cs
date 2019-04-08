//SimpleLoader by Wilson, https://github.com/WilsonPublic/SimpleLoader [Open Source Cheat Loader]
//This is very noob friendly and can easily be adapted, this has no form of protection against cracking so please come up with your own ideas on how to prevent cracking
//I recommend using Dot Net Reactor to protect your programs

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManualMapInjection.Injection;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string HWID;

        private void Form1_Load(object sender, EventArgs e)
        {
            HWID = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value; //Changing the variable "HWID (String)" to the WindowsIdentity Value, you can use any other forms of HWID, you can even use MAC/IP (Not recommended)
            textBox1.Text = HWID;
            textBox1.ReadOnly = true;
            checkonline();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkonline();
            WebClient wb = new WebClient();
            string HWIDLIST = wb.DownloadString("HWID List URL"); //Replace "HWID List URL" with your own URL to a RAW text (txt) file with all your wanted HWIDs [Example: http://myurl.com/HWID.txt]
            if (HWIDLIST.Contains(textBox1.Text)) //You can add a "!" before the "HWIDLIST" and after the "if (" to make it into a blacklist HWID system instead of a whitelist HWID system
            {
                string mainpath = "C:\\Windows\\random.dll"; //You can change the path to wherever you want but just remember to use "\\" instead of just one "\"
                wb.DownloadFile("DLL URL", mainpath); //Replace "DLL URL" with the URL to directly download your DLL [Example: http://myurl.com/MYDLL.dll]
                var name = "csgo"; //Replace "csgo" with any exe you want [Example: For Team Fortress 2 you would replace it with "hl2"]
                var target = Process.GetProcessesByName(name).FirstOrDefault();
                var path = mainpath;
                var file = File.ReadAllBytes(path);

                //Checking if the DLL isn't found
                if (!File.Exists(path))
                {
                    MessageBox.Show("Error: DLL not found");
                    return;
                }
                
                //Injection, just leave this alone if you are a beginner
                var injector = new ManualMapInjector(target) { AsyncInjection = true };
                label2.Text = $"hmodule = 0x{injector.Inject(file).ToInt64():x8}";
                
                if (System.IO.File.Exists(mainpath)) //Checking if the DLL exists
                {
                    System.IO.File.Delete(mainpath); //Deleting the DLL
                }
            }
            else
            {
                MessageBox.Show("HWID Incorrect");
            }
        }

        private void checkonline()
        {
            //Checking if the user can get a response from googles dns server
            try
            {
                //less retarded way of checking for a network connection
                string ip = "8.8.8.8";
                Ping ping = new Ping();
                PingReply reply = ping.Send(ip);

                if (reply.Status == IPStatus.Success)
                {
                    label1.ForeColor = Color.Green;
                    label1.Text = ("Online");
                }
            }
            //catch errors and spit out a simple messagebox
            catch
            {
                //what was the point of setting your label to offline if you are just going to exit the app?
                MessageBox.Show("Could not find a network connection!", "Error", MessageBoxButtons.OK);
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(HWID);
            button2.Enabled = false;
            button2.Text = "Copied";
        }
    }
}
