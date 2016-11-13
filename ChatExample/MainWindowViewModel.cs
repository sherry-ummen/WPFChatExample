using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Akka.Actor;
using Akka.Configuration;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Messages;

namespace ChatExample {
    public class MainWindowViewModel : ViewModelBase {

        private string _chatText;
        private readonly IActorRef _chatClient;
        private readonly string _username = $"{Environment.UserName.ToUpper()}_{DateTime.Now:ddMMyyHHmmss}";
        private readonly ObservableCollection<string> _connectedClients = new ObservableCollection<string>();
        readonly TaskFactory _uiFactory;
        private string _userText;

        public MainWindowViewModel() {
            _uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

            var config = ConfigurationFactory.ParseString(Configuration.HOCONConfig);

            var system = ActorSystem.Create("MyClient", config);
            _chatClient = system.ActorOf(Props.Create<ChatClientActor>());
            ChatClientActor.OnReceive = OnReceive;
            ChatClientActor.OnNewConnect = OnNewConnect;
            ChatClientActor.OnDisconnect = OnDisconnect;
            SendConnectRequest(_chatClient);
        }

        #region Public
        public ObservableCollection<string> ConnectedClients => _connectedClients;

        public string Title { get { return $"Chatty - {_username}"; } }

        public string ChatText {
            get { return _chatText; }
            set { Set(() => ChatText, ref _chatText, value); }
        }

        public string UserText {
            get { return _userText; }
            set { Set(() => UserText, ref _userText, value); }
        }

        public ICommand SendCommand => new RelayCommand<string>(s => SendMessage(s));

        public ICommand CloseCommand => new RelayCommand(Close);
        #endregion Public

        #region Private
        private void OnReceive(SayResponse sayResponse) {
            ChatText += $"[{sayResponse.Username}]\n{sayResponse.Text}\n\n";
        }

        private void OnDisconnect(DisconnectResponse disconnectResponse) {
            if (disconnectResponse.Username != _username) {
                _uiFactory.StartNew(() => {
                    if (_connectedClients.Contains(disconnectResponse.Username))
                        _connectedClients.Remove(disconnectResponse.Username);
                });
            }
        }

        private void OnNewConnect(NewConnect newConnect) {
            _uiFactory.StartNew(() => newConnect.Users.ForEach(x => {
                if (!_connectedClients.Contains(x)) _connectedClients.Add(x);
            }));
        }

        private void SendConnectRequest(IActorRef chatClient) {
            chatClient.Tell(new ConnectRequest() {
                Username = _username,
            });
        }

        private void Close() {
            _chatClient.Tell(new Disconnect() { Username = _username });
        }

        private void SendMessage(string s) {
            if (string.IsNullOrEmpty(s)) return;
            _chatClient.Tell(new SayRequest() {
                Username = _username,
                Text = s,
                DateTime = DateTime.UtcNow
            });
            UserText = string.Empty;
        }
        #endregion Private
    }
}
