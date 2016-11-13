using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Configuration;
using Akka.Util.Internal;
using Messages;

namespace Server {
    class Program {
        static void Main(string[] args) {
            var config = ConfigurationFactory.ParseString(Configuration.HOCONConfigServer);

            using (var system = ActorSystem.Create("MyServer", config)) {
                system.ActorOf<ChatServerActor>("ChatServer");
                Console.ReadLine();
            }
        }
    }

    class ChatServerActor : TypedActor,
        IHandle<SayRequest>,
        IHandle<ConnectRequest>,
        IHandle<Disconnect>,
        ILogReceive {

        private readonly HashSet<IActorRef> _clients = new HashSet<IActorRef>();
        private readonly HashSet<string> _clientsIds = new HashSet<string>();

        public void Handle(SayRequest message) {
            Console.WriteLine("User {0} said {1}", message.Username, message.Text);
            var response = new SayResponse {
                Username = message.Username,
                Text = message.Text,
                DateTime = message.DateTime
            };

            foreach (var client in _clients) client.Tell(response, Self);
        }

        public void Handle(ConnectRequest message) {

            Console.WriteLine("User {0} has connected", message.Username);
            _clients.Add(this.Sender);
            _clientsIds.Add(message.Username);
            Sender.Tell(new ConnectResponse {
                Message = "Hello and welcome to the chat room",
            }, Self);
            _clients.ForEach(x => x.Tell(new NewConnect() {Users = _clientsIds.ToList()}));
        }

        public void Handle(Disconnect message){
            _clientsIds.Remove(message.Username);
            foreach (var client in _clients) client.Tell(new DisconnectResponse() {Username = message.Username}, Self);
        }
    }
}
