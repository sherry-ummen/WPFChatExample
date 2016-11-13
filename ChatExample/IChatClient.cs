using Akka.Actor;
using Messages;

namespace ChatExample {
    internal interface IChatClient : IHandle<ConnectRequest>,
                                        IHandle<ConnectResponse>,
                                        IHandle<SayRequest>,
                                        IHandle<SayResponse>,
                                        IHandle<Disconnect>,
                                        IHandle<DisconnectResponse>{

    }
}
