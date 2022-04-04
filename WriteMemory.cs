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
    class WriteMemory
    {
        MemoryHelper64 helper;
        ulong Level;
        ulong Vigueur;
        ulong baseAddr;
        int[] offsetLvl = { 0x8, 0x68 };
        int[] offsetVigor = { 0x8, 0x3C };
        Process p;
        public WriteMemory()
        {
            p = Process.GetProcessesByName("eldenring").FirstOrDefault();
            if (p != null)
            {
                helper = new MemoryHelper64(p);
                baseAddr = helper.GetBaseAddress(0x3C5CD78);
                Level = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetLvl);
                Vigueur = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetVigor);
            }
        }
        public void WriteOnMemory(string number)
        {
            if (p != null)
            {
                helper.WriteMemory(Vigueur, Convert.ToInt32(number));
            }
        }
    }
}

