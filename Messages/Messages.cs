using System;
using System.Collections.Generic;

namespace Messages {
    public class ConnectRequest {
        public string Username { get; set; }
    }

    public class ConnectResponse {
        public string Message { get; set; }
    }

    public class SayRequest {
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class SayResponse {
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class Disconnect {
        public string Username { get; set; }
    }

    public class DisconnectResponse {
        public string Username { get; set; }
    }

    public class NewConnect{
        public List<string> Users { get; set; }
    }
}
