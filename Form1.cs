using Memory.Utils;
using Memory.Win64;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchLib.Client;
using WebSocketSharp;

namespace Tricherie
{
    public partial class Form1 : Form
    {
        bool isBotHere = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
           if(isBotHere == false)
            {
                Bot bot = new Bot();
                isBotHere = true;
            } 
        }

            
    }
}
