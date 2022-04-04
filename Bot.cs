using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        readonly string vigueur = "vigueur";
        readonly string esprit = "esprit";
        readonly string endurance = "endurance";
        readonly string force = "force";
        readonly string dexterite = "dexterite";
        readonly string intelligence = "intelligence";
        readonly string foi = "foi";
        readonly string esoterisme = "esoterisme";

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
                Debug.WriteLine(splitString[3]);
                memoryWriter.WriteOnMemory(splitString[3]);
                client.SendMessage("siul008", "Ma Vigueur devient " + splitString[3]);
            }
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Voici lé croupié");
            client.SendMessage(e.Channel, "Voici lé croupié");
        }
        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.StartsWith("!tomate") && e.ChatMessage.IsSubscriber)
            {
                client.SendMessage("siul008", "vous etes stupides");
            }
        }

    }
}
