using System;
using Akka.Actor;
using Messages;

namespace ChatExample {
    public class ChatClientActor: TypedActor, IChatClient {

        private readonly ActorSelection _server = Context.ActorSelection(Configuration.ConnnectionString);

        public static Action<SayResponse> OnReceive { get; set; }
        public static Action<ConnectResponse> OnConnect { get; set; }
        public static Action<NewConnect> OnNewConnect { get; set; }
        public static Action<DisconnectResponse> OnDisconnect { get; set; }

        public void Handle(ConnectRequest message){
            _server.Tell(message);
        }

        public void Handle(ConnectResponse message){
            OnConnect?.Invoke(message);
        }

        public void Handle(NewConnect message) {
            OnNewConnect?.Invoke(message);
        }

        public void Handle(SayRequest message){
           _server.Tell(message);
        }

        public void Handle(SayResponse message){
           OnReceive?.Invoke(message);
        }

        public void Handle(Disconnect message){
            _server.Tell(message);
        }

        public void Handle(DisconnectResponse message){
            OnDisconnect?.Invoke(message);
        }
    }
}
