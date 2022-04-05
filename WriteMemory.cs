using Memory.Utils;
using Memory.Win64;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
       public readonly string folderpath = @"C:\Users\"+ Environment.UserName + @"\Documents\EldenRingTwitchIntegration\";
       public  readonly string vigueurTxt = "vigueur.txt", enduranceTxt = "endurance.txt", espritTxt = "esprit.txt", forceTxt = "force.txt", dexTxt = "dex.txt",
            intelTxt = "intel.txt", foiTxt = "foi.txt", esoTxt = "eso.txt";
        MemoryHelper64 helper;
        ulong Level, Vigueur, Endurance, Esprit, Force, Dexterite, Intelligence, Foi, Esoterisme, hpActuel;
        int oldLevel, oldVigueur, oldEndurance, oldEsprit, oldForce, oldDexterite, oldIntelligence, oldFoi, oldEsoterisme;
        ulong statsBaseAddr, hpBaseAddr;
        int[] offsetLvl = { 0x8, 0x68 };
        int[] offsetVigor = { 0x8, 0x3C };
        int[] offsetEndu = { 0x8, 0x44 };
        int[] offsetEsprit = { 0x8, 0x40 };
        int[] offsetForce = { 0x8, 0x48 };
        int[] offsetDex = { 0x8, 0x4C };
        int[] offsetIntel = { 0x8, 0x50 };
        int[] offsetFoi = { 0x8, 0x54 };
        int[] offsetEso = { 0x8, 0x58 };
        int[] offsetHp = { 0x0, 0x190, 0x0, 0x138 };
        int modif;
        Process p;
        bool error = false;
        string finalMessage;
        public WriteMemory()
        {
            p = Process.GetProcessesByName("eldenring").FirstOrDefault();
            if (p != null)
            {
                helper = new MemoryHelper64(p);
                statsBaseAddr = helper.GetBaseAddress(0x3C5CD78);
                hpBaseAddr = helper.GetBaseAddress(0x3A2ED50);
                Level = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetLvl);
                Vigueur = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetVigor);
                Endurance = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetEndu);
                Esprit = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetEsprit);
                Force = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetForce);
                Dexterite = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetDex);
                Intelligence = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetIntel);
                Foi = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetFoi);
                Esoterisme = MemoryUtils.OffsetCalculator(helper, statsBaseAddr, offsetEso);                
                WriteStatsInFile();               
            }
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            if (!File.Exists(folderpath + vigueurTxt))
            {
                File.CreateText(folderpath + vigueurTxt);
                File.CreateText(folderpath + enduranceTxt);
                File.CreateText(folderpath + espritTxt);
                File.CreateText(folderpath + forceTxt);
                File.CreateText(folderpath + dexTxt);
                File.CreateText(folderpath + intelTxt);
                File.CreateText(folderpath + foiTxt);
                File.CreateText(folderpath + esoTxt);
            }
        }
        public void WriteOnMemoryStats(string numberString, string modifier, bool isAdding)
        {
            if (p != null)
            {
                int number = Convert.ToInt32(numberString);
                if (isAdding)
                {
                    number = 1;
                    finalMessage = "Tu as ajouté 1 à ";
                }
                else if (isAdding == false)
                {
                    number = -1;
                    finalMessage = "Tu as réduis de 1 ";
                }
                if (number == 0)
                {
                    number = 0;
                    finalMessage = "Tu n'as rien changé à ";
                }
              
                switch (modifier)
                {
                    case "vigueur":
                        oldVigueur = helper.ReadMemory<int>(Vigueur);
                        if ((oldVigueur + number) < 1)
                        {
                            modif = 1;
                            finalMessage = $"La stat : {modifier} est déja à 1, je reste à 1";
                        }
                        else
                        {
                            modif = oldVigueur + number;
                            finalMessage += $"la stat : {modifier} !";
                        }
                        helper.WriteMemory(Vigueur, modif);
                        Debug.WriteLine("oldVigueur :" + oldVigueur + " + " + modif + $" = {helper.ReadMemory<int>(Vigueur)}");
                        break;

                    case "esprit":
                        oldEsprit = helper.ReadMemory<int>(Esprit);
                        if ((oldEsprit + number) < 1)
                        {
                            modif = 1;
                            finalMessage = $"la stat : {modifier} est déja à 1, je reste à 1";
                        }
                        else
                        {
                            modif = oldEsprit + number;
                            finalMessage += $"la stat : {modifier} !";
                        }
                        helper.WriteMemory(Esprit, modif);
                        break;

                    case "endurance":
                        oldEndurance = helper.ReadMemory<int>(Endurance);
                        if ((oldEndurance + number) < 1)
                        {
                            modif = 1;
                            finalMessage = $"La stat : {modifier} est déja à 1, je reste à 1";
                        }
                        else
                        {
                            modif = oldEndurance + number;
                            finalMessage += $"la stat : {modifier} !";
                        }

                        helper.WriteMemory(Endurance, modif);
                        break;

                    case "force":
                        oldForce = helper.ReadMemory<int>(Force);
                        if ((oldForce + number) < 5)
                        {
                            modif = 5;
                            finalMessage = $"La stat : {modifier} est déja à 5, je reste à 5";
                        }
                        else
                        {
                            modif = oldForce + number;
                            finalMessage += $"la stat : {modifier} !";
                        }

                        helper.WriteMemory(Force, modif);
                        break;

                    case "dexterite":
                        oldDexterite = helper.ReadMemory<int>(Dexterite);
                        if ((oldDexterite + number) < 5)
                        {
                            modif = 5;
                            finalMessage = $"La stat : {modifier} est déja à 5, je reste à 5";
                        }
                        else
                        {
                            modif = oldDexterite + number;
                            finalMessage += $"la stat : {modifier} !";
                        }

                        helper.WriteMemory(Dexterite, modif);
                        break;

                    case "foi":
                        oldFoi = helper.ReadMemory<int>(Foi);
                        if ((oldFoi + number) < 1)
                        {
                            modif = 1;
                            finalMessage = $"La stat : {modifier} est déja à 1, je reste à 1";
                        }
                        else
                        { 
                            modif = oldFoi + number;
                            finalMessage += $"la stat : {modifier} !";
                        }
                        helper.WriteMemory(Foi,modif);
                        break;

                    case "intelligence":
                        oldIntelligence = helper.ReadMemory<int>(Intelligence);
                        if ((oldIntelligence + number) < 1)
                        {
                            modif = 1;
                            finalMessage = $"La stat : {modifier} est déja à 1, je reste à 1";
                        }
                        else
                        {
                            modif = oldIntelligence + number;
                            finalMessage += $"la stat : {modifier} !";
                        }
                        helper.WriteMemory(Intelligence,modif);
                        break;

                    case "esoterisme":
                        oldEsoterisme = helper.ReadMemory<int>(Esoterisme);
                        if((oldEsoterisme + number) < 1)
                        {
                            modif = 1;
                            finalMessage = $"La stat : {modifier} est déja à 1, je reste à 1";
                        }
                        else 
                        {
                            modif = oldEsoterisme + number;
                            finalMessage += $"la stat : {modifier} !";
                        }
                        helper.WriteMemory(Esoterisme,modif);
                        break;
                }
                Bot.CreateMessage(finalMessage);
                WriteStatsInFile();
                finalMessage = "";
            }
        }
        public void DamageAtmHp(string damageString)
        {
            hpActuel = MemoryUtils.OffsetCalculator(helper, hpBaseAddr, offsetHp);
            int damage = Convert.ToInt32(damageString);
            int hp = helper.ReadMemory<int>(hpActuel);
            if((hp - damage < 0))
            {
                helper.WriteMemory(hpActuel, 20);
            }
            else if(damage == 0 || damage < 0)
            {
                helper.WriteMemory(hpActuel, hp - 10);
                Bot.CreateMessage("Étrange demande");
            }
            else
            {
                hp -= damage;
                helper.WriteMemory(hpActuel, hp);
            }
        }
        public void HealAtmHP(string healString)
        {
            hpActuel = MemoryUtils.OffsetCalculator(helper, hpBaseAddr, offsetHp);
            int heal = Convert.ToInt32(healString);
            int hp = helper.ReadMemory<int>(hpActuel);
            if(heal > 500)
            {
                heal = 500;
            }
            else if(heal < 0)
            {
                heal = 5;
            }
            helper.WriteMemory(hpActuel, hp+heal);
        }
        public void WriteStatsInFile()
        {
            if(p != null)
            {
                File.WriteAllText(folderpath + vigueurTxt, helper.ReadMemory<int>(Vigueur).ToString());
                File.WriteAllText(folderpath + espritTxt, helper.ReadMemory<int>(Esprit).ToString());
                File.WriteAllText(folderpath + enduranceTxt, helper.ReadMemory<int>(Endurance).ToString());
                File.WriteAllText(folderpath + forceTxt, helper.ReadMemory<int>(Force).ToString());
                File.WriteAllText(folderpath + dexTxt, helper.ReadMemory<int>(Dexterite).ToString());
                File.WriteAllText(folderpath + foiTxt, helper.ReadMemory<int>(Foi).ToString());
                File.WriteAllText(folderpath + intelTxt, helper.ReadMemory<int>(Intelligence).ToString());
                File.WriteAllText(folderpath + esoTxt, helper.ReadMemory<int>(Esoterisme).ToString());
            }
        }
    }
}

