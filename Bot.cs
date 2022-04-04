using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace Tricherie
{
    class Bot
    {
        TwitchClient client;
        readonly string musique = "custom-reward-id=f88c3bed-a091-4b9b-abd3-a27d7a684070";
        WriteMemory memoryWriter;
        string[] allStats = new string[] { "vigueur", "esprit", "endurance", "force", "dexterite", "intelligence", "foi", "esoterisme" };
        string stats;
        string message;
        string messageChoix;
        string messageValeur;
        bool erreurBool = false;

        public Bot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials(Properties.Resources.username, Properties.Resources.token);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, "siul008");

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnConnected += Client_OnConnected;
            client.OnMessageReceived += Client_OnMessageReceived;

            client.Connect();

            memoryWriter = new WriteMemory();
        }

            private void Client_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
            
            
            if(e.Data.Contains(musique))
            {
                var myString = e.Data.ToString();
                var splitString = myString.Split(':');
                message = splitString[3].ToLower();
                Debug.WriteLine(message);
                ChangeStats(message);
            }
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Voici lé croupié");
            client.SendMessage(e.Channel, "Je suis un bot siul00Hi");
        }
        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.StartsWith("!tomate") && e.ChatMessage.IsSubscriber)
            {
                client.SendMessage("siul008", "vous etes stupides");
            }
        }

        private void ChangeStats(string messageInput)
        {
            var messageSplit = message.Split('.');
            messageChoix = messageSplit[0];
            messageValeur = messageSplit[1];
            if (!Regex.IsMatch(messageValeur, @"[a-zA-Z]"))
            {
                if (messageChoix.Contains("vig"))

                { stats = allStats[0]; }

                else if (messageChoix.Contains("esp") || (messageChoix.Contains("mind")))

                { stats = allStats[2]; }

                else if (messageChoix.Contains("end"))

                { stats = allStats[1]; }

                else if (messageChoix.Contains("force") || (messageChoix.Contains("str")))

                { stats = allStats[3]; }

                else if (messageChoix.Contains("dex"))

                { stats = allStats[4]; }

                else if (messageChoix.Contains("int"))

                { stats = allStats[5]; }

                else if (messageChoix.Contains("foi") || (messageChoix.Contains("faith")))

                { stats = allStats[6]; }

                else if (messageChoix.Contains("eso") || (messageChoix.Contains("arcane")))

                { stats = allStats[7]; }

                else
                {
                    client.SendMessage("siul008", $"Erreur de Syntaxe à : ( {messageChoix} ), le message doit ressembler à : vigueur:20");
                    erreurBool = true;
                }

                if(erreurBool == false)
                {
                    client.SendMessage("siul008", $"Ma {stats} devient " + messageValeur);
                    memoryWriter.WriteOnMemoryStats(messageValeur, stats);
                }
            }
            else
            {
                client.SendMessage("siul008", $"Erreur de Syntaxe à : ( {messageValeur} ), le message doit ressembler à : vigueur:20");
            }
        }

    }
}
