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
        ulong Level, Vigueur, Endurance, Esprit, Force, Dexterite, Intelligence, Foi, Esoterisme;
        int oldLevel, oldVigueur, oldEndurance, oldEsprit, oldForce, oldDexterite, oldIntelligence, oldFoi, oldEsoterisme;
        ulong baseAddr;
        int[] offsetLvl = { 0x8, 0x68 };
        int[] offsetVigor = { 0x8, 0x3C };
        int[] offsetEndu = { 0x8, 0x44 };
        int[] offsetEsprit = { 0x8, 0x40 };
        int[] offsetForce = { 0x8, 0x48 };
        int[] offsetDex = { 0x8, 0x50 };
        int[] offsetIntel = { 0x8, 0x3C };
        int[] offsetFoi = { 0x8, 0x54 };
        int[] offsetEso = { 0x8, 0x58 };
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
                Endurance = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetEndu);
                Esprit = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetEsprit);
                Force = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetForce);
                Dexterite = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetDex);
                Intelligence = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetIntel);
                Foi = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetFoi);
                Esoterisme = MemoryUtils.OffsetCalculator(helper, baseAddr, offsetEso);
            }
        }
        public void WriteOnMemoryStats(string numberString, string modifier)
        {
            if (p != null)
            {
                int number = Convert.ToInt32(numberString);
                if (number > 99)
                {
                    number = 99;
                }
                else if(number < 1)
                {
                    number = 1;
                }
                switch (modifier)
                {
                    case "vigueur":
                        oldVigueur = helper.ReadMemory<int>(Vigueur);
                        helper.WriteMemory(Vigueur, number);
                        MessageBox.Show(oldVigueur.ToString());
                        break;

                    case "esprit":
                        helper.WriteMemory(Esprit, number);
                        break;

                    case "endurance":
                        helper.WriteMemory(Endurance, number);
                        break;

                    case "force":
                        helper.WriteMemory(Force, number);
                        break;

                    case "dexterite":
                        helper.WriteMemory(Dexterite, number);
                        break;

                    case "foi":
                        helper.WriteMemory(Foi, number);
                        break;

                    case "intelligence":
                        helper.WriteMemory(Intelligence, number);
                        break;

                    case "esoterisme":
                        helper.WriteMemory(Esoterisme, number);
                        break;
                }
            }
        }
    }
}

